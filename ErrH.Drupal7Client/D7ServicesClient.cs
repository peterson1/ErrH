using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7Client.SessionAuthentication;
using ErrH.Drupal7Client.StatusMessages;
using ErrH.Drupal7Client.TaxonomyTerms;
using ErrH.RestSharpShim;
using ErrH.Tools.Authentication;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.RestServiceShim.RestExceptions;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client
{
    public class D7ServicesClient : LogSourceBase, ID7Client
    {
        private      EventHandler<UserEventArg> _loggedOut;
        private      EventHandler<UserEventArg> _loggedIn;

        private TaxoTermsLoader _termLoadr = new TaxoTermsLoader();
        private IClientShim     _client;
        private SessionAuth     _auth;
        private IFileSystemShim _fsShim;
        private ISerializer     _serialzr;

        public int RetryIntervalSeconds { get; set; } = 10;

        public D7Term               Term   
                            (int tid, bool errorIfMissing) => _termLoadr.Term(tid, errorIfMissing);
        public IEnumerable<D7Term>  Terms                  => _termLoadr.Terms;
        public bool                 IsTermsLoaded          => _termLoadr.IsLoaded;
        public Task<bool> LoadTerms(CancellationToken tkn) => _termLoadr.Load(this, tkn);

        public event EventHandler<EArg<bool>>    ResponseReceived;
        public event EventHandler<UserEventArg>  LoggedIn
        {
            add    { _loggedIn -= value; _loggedIn += value; }
            remove { _loggedIn -= value; }
        }
        public event EventHandler<UserEventArg>  LoggedOut
        {
            add    { _loggedOut -= value; _loggedOut += value; }
            remove { _loggedOut -= value; }
        }



        //private bool _loginStarted = false;
        //private static readonly Object obj = new Object();


        public D7ServicesClient(IFileSystemShim fsShim, ISerializer serializer)
        {
            _fsShim   = ForwardLogs(fsShim);
            _serialzr = ForwardLogs(serializer);
            _client   = ForwardLogs(new RestSharpClientShim());
            _auth     = ForwardLogs(new SessionAuth(fsShim, serializer));

            _client.ResponseReceived += (s, e) 
                => RaiseResponseReceived(e.Value);
        }



        public async Task<bool> Login(string baseUrl, 
                                      string userName, 
                                      string password,
                                      CancellationToken cancelToken)
        {
            Trace_n($"Logging in as “{userName}”...", "server: " + baseUrl);

            _client.BaseUrl = baseUrl;
            if (this.IsLoggedIn) goto FireLoggedIn;


            try {
                await _auth.OpenNewSession(_client, userName, password, cancelToken);
            }
            catch (RestServiceException ex) { OnLogin.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }
            if (!IsLoggedIn) return Error_n("Failed to authenticate!", "");

        FireLoggedIn:
            _loggedIn?.Invoke(this, EventArg.User(userName));
            return Trace_n("Successfully logged in.", "");
        }

        public async Task<bool> Login(IBasicAuthenticationKey creds, CancellationToken cancelToken)
            => await Login(creds.BaseUrl, creds.UserName, creds.Password, cancelToken);


        public string BaseUrl    => _client.BaseUrl;
        public bool   IsLoggedIn => _auth.IsLoggedIn;



        public async Task<bool> Logout(CancellationToken cancelToken)
        {
            if (!IsLoggedIn) return true;

            DeleteSavedSession();

            var usr = _auth.Current.user.name;
            var ok = await _auth.CloseSession(_client, cancelToken);

            if (ok) _loggedOut?.Invoke(this, EventArg.User(usr));
            return ok;
        }


        public async Task<T> Get<T>(string resource,
                                    CancellationToken cancelToken,
                                    string taskTitle,
                                    string successMsg,
                                    params Func<T, object>[] successMsgArgs
                                    ) where T : new()
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            var req = _auth.Req.GET(resource);
            try
            {
                return await _client.Send<T>(req, cancelToken, taskTitle, successMsg, successMsgArgs);
            }
            catch (RestServiceException ex) { OnGet.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }
            return default(T);
        }



        public async Task<int> Post(FileShim file,
                                    CancellationToken cancelToken,
                                    string serverFoldr,
                                    bool isPrivate)
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            Trace_n("Uploading file to server...", "");
            var req = _auth.Req.POST(URL.Api_FileJson);

            req.Body = new D7File_Out(file,
                            serverFoldr, isPrivate);

            D7File d7f = null; try
            {
                d7f = await _client.Send<D7File>(req, cancelToken, null,
                    "Successfully uploaded “{0}” ({1}) [fid: {2}].",
                        x => file.Name, x => file.Size.KB(), x => x.fid);
            }
            catch (RestServiceException ex) { OnFileUpload.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }


            if (d7f != null && d7f.fid > 0) return d7f.fid;
            else if (d7f == null) return Error_(-1, "Returned null.", "");
            else return Error_(-1, "Unexpected file id: " + d7f.fid, "");
        }



        public async Task<T> Post<T>(T d7Node, 
                                     CancellationToken cancelToken) 
            where T : D7NodeBase, new()
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            if (_client == null) throw Error.NullRef("_client");
            IRequestShim req = null;
            try {
                req = _auth.Req.POST(URL.Api_EntityNode);
                d7Node.uid = this.CurrentUser.uid;
                req.Body = d7Node;
            }
            catch (Exception ex) { LogError("req = _auth.Req.POST", ex); return null; }



            Trace_n("Creating new node on server...", "");
            T d7n = default(T); string m;
            try
            {
                //d7n = await _client.Send<T>(req, cancelToken, "",
                //    "Successfully created new «{0}»: [nid: {1}] “{2}”",
                //        x => x?.type, x => x?.nid, x => x?.title);
                d7n = await _client.Send<T>(req, cancelToken);
            }
            catch (RestServiceException ex) { OnNodeAdd.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                return d7n;
            else
                return Error_(d7n, "Invalid node.", m);
        }


        public async Task<T> Node<T>(int nodeId, 
                                     CancellationToken cancelToken) 
            where T : D7NodeBase, new()
        {
            T d7n = default(T); string m;

            if (!IsLoggedIn)
                return Error_(d7n, $"{GetType().Name}.Node<T>", "Not logged in.");

            var req = _auth.Req.GET(URL.Api_EntityNodeX, nodeId);

            Trace_n("Getting node (id: {0}) from server...".f(nodeId), "type: " + typeof(T).Name.Guillemet());
            try
            {
                d7n = await _client.Send<T>(req, cancelToken);
            }
            catch (RestServiceException ex) { OnNodeGet.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                return Trace_(d7n, "Node successfully retrieved.", 
                    $"[nid: {nodeId}] ‹{d7n.type}› title: “{d7n.title}”");
            else
                return Error_(d7n, "Invalid node.", m);
        }


        public D7User CurrentUser => _auth?.Current?.user;


        public async Task<T> Put<T>(T nodeRevision,
                                    CancellationToken cancelToken,
                                    string taskTitle = null, 
                                    string successMessage = null, 
                                    params Func<T, object>[] successMsgArgs) 
            where T : ID7NodeRevision
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            Trace_n(taskTitle.IsBlank() ? "Updating existing node on server..." : taskTitle, "");

            string m; T d7n = default(T);
            if (nodeRevision.vid < 1)
                return Error_(d7n, "Invalid node revision format.", "Revision ID (vid) must be set.");

            var req = _auth.Req.PUT(URL.Api_EntityNodeX, nodeRevision.nid);
            nodeRevision.uid = this.CurrentUser.uid;
            req.Body = nodeRevision;

            try
            {
                d7n = await _client.Send<T>(req, cancelToken, "", successMessage, successMsgArgs);
            }
            catch (RestServiceException ex) { OnNodeEdit.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                //return Trace_(d7n, "Node successfully updated.", "[nid: {0}] {1} title: {2}", d7n.nid, d7n.type.Guillemet(), d7n.title.Quotify());
                return d7n;
            else
                return Error_(d7n, "Invalid node.", m);
        }


        public async Task<bool> Put (ID7NodeRevision nodeRevision,
                                     CancellationToken tkn)
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            //Trace_n("", _serialzr.Write(nodeRevision, false));
            if (nodeRevision.vid < 1)
                return Error_n("Invalid node revision format.", "Revision ID (vid) must be set.");

            var req = _auth.Req.PUT(URL.Api_EntityNodeX, nodeRevision.nid);
            nodeRevision.uid = this.CurrentUser.uid;
            req.Body = nodeRevision;

            Debug_n($"Updating node [nid:{nodeRevision.nid}]...", "");

            IResponseShim resp = null; try
            {
                resp = await _client.Send(req, tkn);
            }
            catch (RestServiceException ex) { OnNodeEdit.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (resp == null)
                return Error_n("Failed to update node.", "Response is NULL");

            if (!resp.IsSuccess)
                return Error_n("Failed to update node.", resp.Message);

            return Debug_n("Node successfully updated.", "");
        }



        public async Task<bool> Delete(int nid, 
                                       CancellationToken cancelToken)
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            var req = _auth.Req.DELETE(URL.Api_EntityNodeX, nid);

            Trace_n("Deleting node from server...", "nid: " + nid);
            IResponseShim resp = null; try
            {
                resp = await _client.Send(req, cancelToken);
            }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (resp == null)
                return Error_n("Unexpected NULL response.", "‹IResponseShim›");

            if (!resp.IsSuccess)
                return OnNodeDelete.Err(this, (RestServiceException)resp.Error);

            if (resp.Content != "null")
                Warn_n("Unexpected response content.", $"“{resp.Content}”");

            return Trace_n("Node successfully deleted from server.", resp.Content);
        }


        //
        //  no need to delete the actual file by fid
        //    - Drupal 7 auto-deletes it when losing the reference to a node
        //
        public async Task<bool> DeleteFile(int fid,
                                           CancellationToken cancelToken)
        {
            if (!IsLoggedIn)
                throw Error.BadAct($"‹{this.GetType().Name}› is not logged in.");

            var req = _auth.Req.DELETE(URL.Api_FileX, fid);

            Trace_n("Deleting file from server...", "fid: " + fid);
            IResponseShim resp = null; try
            {
                resp = await _client.Send(req, cancelToken);
            }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (resp == null)
                return Error_(false, "Unexpected NULL response.", "IResponseShim".Guillemets());

            if (!resp.IsSuccess)
                return OnFileDelete.Err(this, (RestServiceException)resp.Error);

            if (resp.Content != "[true]")
                return Error_(false, "File probably in use by a node.", resp.Content);

            return Trace_n("File successfully deleted from server.", resp.Content);
        }



        public void DeleteSavedSession() => _auth.SessionFile.Delete();
        public bool HasSavedSession      => _auth.SessionFile.Found;
        public void SaveSession()
        {
            if (!IsLoggedIn)
            {
                Warn_n("Illogical operation attempted.", "Illogical to save session while disconnected.");
                return;
            }
            //SessionAuthFile.Write(_auth.Current, _fsShim, _serialzr);
            _auth.WriteSessionFile();
        }


        public void LoadSession()
        {
            //var session = SessionAuthFile.Read(_fsShim, _serialzr);
            var session = _auth.ReadSessionFile();
            if (session == null) Warn_n("Failed to load session.", "Reading SessionAuthFile returned NULL.");
            _auth.Current = session;
            _client.BaseUrl = session.BaseURL;
            if (IsLoggedIn)
                _loggedIn?.Invoke(this, EventArg.User(session?.user?.name));
        }



        public bool LocalizeSessionFile(IBasicAuthenticationKey authKey)
        {
            if (authKey.BaseUrl.IsBlank() || authKey.UserName.IsBlank())
                return Warn_n("Cannot localize session file.",
                        "BaseURL / UserName of AuthKey is blank.");


            var loc  = _fsShim.GetSpecialDir(SpecialDir.LocalApplicationData);
            var typ  = this.GetType().Name;
            var dom  = authKey.BaseUrl.TextAfter("//").Replace(":", "-");
            var usr  = authKey.UserName;//.Replace(" ", "_");
            var end  = "user.session";
            var path = loc.Bslash(typ).Bslash(dom).Bslash(usr).Bslash(end);
            _auth.SessionFile = _fsShim.File(path);

            return true;
        }


        protected void RaiseResponseReceived(bool isSuccess)
            => ResponseReceived?.Invoke(this, new EArg<bool> { Value = isSuccess });


        /*public async void LoginUsingCredentials(object sender, EArg<LoginCredentials> evtArg)
        {
            var e = evtArg.Value;
            RetryIntervalSeconds = e.RetryIntervalSeconds;
            _client.BaseUrl = e.BaseUrl;

            while (true)
            {
                LoadSession();

                if (!IsLoggedIn)
                    await this.Login(e.BaseUrl, e.Name, e.Password);

                //if (IsLoggedIn) SaveSession();
                if (IsLoggedIn) return;

                Trace_h("Unable to login.", "Retrying after {0:second} . . .", RetryIntervalSeconds);
                await TaskEx.Delay(1000 * RetryIntervalSeconds);
            }
        }*/

    }
}
