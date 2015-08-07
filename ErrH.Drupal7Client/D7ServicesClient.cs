﻿using System;
using System.Threading.Tasks;
using ErrH.Drupal7Client.SessionAuthentication;
using ErrH.Drupal7Client.StatusMessages;
using ErrH.RestSharpShim;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client
{
    public class D7ServicesClient : LogSourceBase, ID7Client
    {
        //later: centralize these
        const string _api_file_json = "/api/file.json";
        const string _api_file_x = "/api/file/{0}";
        //const string _api_node_x        = "/api/node/{0}";
        //const string _api_node_json     = "/api/node.json";
        //const string _api_node_x_json   = "/api/node/{0}.json";
        const string _api_node_x_attach_file = "/api/node/{0}/attach_file";

        const string _api_entity_node = "/api/entity_node.json";
        const string _api_entity_node_x = "/api/entity_node/{0}.json";
        const string _api_entity_node_x_attach_file = "/api/entity_node/{0}/attach_file";

        private IClientShim _client;
        private SessionAuth _auth;
        private IFileSystemShim _fsShim;
        private ISerializer _serialzr;

        //private bool _loginStarted = false;
        //private static readonly Object obj = new Object();


        public D7ServicesClient(IFileSystemShim fsShim, ISerializer serializer)
        {
            this._fsShim = ForwardLogs(fsShim);
            this._serialzr = ForwardLogs(serializer);
            this._client = ForwardLogs(new RestSharpClientShim());
            this._auth = ForwardLogs(new SessionAuth());
        }



        public async Task<bool> Login(string baseUrl, string userName, string password)
        {
            _client.BaseUrl = baseUrl;
            if (this.IsLoggedIn) return true;

            Trace_n("Logging in as " + userName.Quotify() + "...", "server: " + _client.BaseUrl);

            try
            {
                await _auth.OpenNewSession(_client, userName, password);
            }
            catch (RestServiceException ex) { OnLogin.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (this.IsLoggedIn)
                return Trace_n("Successfully logged in.", "");
            else
                return Error_n("Failed to authenticate!", "");
        }



        public string BaseUrl { get { return _client.BaseUrl; } }
        public bool IsLoggedIn { get { return _auth.IsLoggedIn; } }



        public async Task<bool> Logout()
        {
            return await _auth.CloseSession(_client);
        }


        public async Task<T> Get<T>(string resource,
                                    string taskTitle,
                                    string successMsg,
                                    params Func<T, object>[] successMsgArgs
                                    ) where T : new()
        {
            var req = _auth.Req.GET(resource);
            try
            {
                return await _client.Send<T>(req, taskTitle, successMsg, successMsgArgs);
            }
            catch (RestServiceException ex) { OnGet.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }
            return default(T);
        }



        public async Task<int> Post(FileShim file,
                                    string serverFoldr,
                                    bool isPrivate)
        {
            Trace_n("Uploading file to server...", "");
            var req = _auth.Req.POST(_api_file_json);

            req.Body = new D7File_Out(file,
                            serverFoldr, isPrivate);

            D7File d7f = null; try
            {
                d7f = await _client.Send<D7File>(req, null,
                    "Successfully uploaded “{0}” ({1}) [fid: {2}].",
                        x => file.Name, x => file.Size.KB(), x => x.fid);
            }
            catch (RestServiceException ex) { OnFileUpload.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }


            if (d7f != null && d7f.fid > 0) return d7f.fid;
            else if (d7f == null) return Error_(-1, "Returned null.", "");
            else return Error_(-1, "Unexpected file id: " + d7f.fid, "");
        }



        public async Task<T> Post<T>(T d7Node) where T : D7NodeBase, new()
        {
            //Trace_n("Creating new node on server...", "{0} to {1}", d7Node.TypeName(), d7Node.type.Guillemet());
            Trace_n("Creating new node on server...", "");
            var req = _auth.Req.POST(_api_entity_node);
            d7Node.uid = this.CurrentUser.uid;
            req.Body = d7Node;

            T d7n = default(T); string m;
            try
            {
                d7n = await _client.Send<T>(req, "",
                    "Successfully created new «{0}»: [nid: {1}] “{2}”",
                        x => x.type, x => x.nid, x => x.title);
            }
            catch (RestServiceException ex) { OnNodeAdd.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                return d7n;
            else
                return Error_(d7n, "Invalid node.", m);
        }


        public async Task<T> Node<T>(int nodeId) where T : D7NodeBase, new()
        {
            T d7n = default(T); string m;
            var req = _auth.Req.GET(_api_entity_node_x, nodeId);

            Trace_n("Getting node (id: {0}) from server...".f(nodeId), "type: " + typeof(T).Name.Guillemet());
            try
            {
                d7n = await _client.Send<T>(req);
            }
            catch (RestServiceException ex) { OnNodeGet.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                return Trace_(d7n, "Node successfully retrieved.", 
                    $"[nid: {nodeId}] ‹{d7n.type}› title: “{d7n.title}”");
            else
                return Error_(d7n, "Invalid node.", m);
        }


        public D7User CurrentUser
        {
            get
            {
                if (_auth == null) return null;
                if (_auth.Current == null) return null;
                return _auth.Current.user;
            }
        }




        public async Task<T> Put<T>(T nodeRevision, string taskTitle = null, string successMessage = null, params Func<T, object>[] successMsgArgs) where T : ID7NodeRevision, new()
        {
            Trace_n(taskTitle.IsBlank() ? "Updating existing node on server..." : taskTitle, "");

            string m; T d7n = default(T);
            if (nodeRevision.vid < 1)
                return Error_(d7n,
"Invalid node revision format.", "Revision ID (vid) must be set.");

            var req = _auth.Req.PUT(_api_entity_node_x, nodeRevision.nid);
            nodeRevision.uid = this.CurrentUser.uid;
            req.Body = nodeRevision;

            try
            {
                d7n = await _client.Send<T>(req, "", successMessage, successMsgArgs);
            }
            catch (RestServiceException ex) { OnNodeEdit.Err(this, ex); }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (d7n.IsValidNode(out m))
                //return Trace_(d7n, "Node successfully updated.", "[nid: {0}] {1} title: {2}", d7n.nid, d7n.type.Guillemet(), d7n.title.Quotify());
                return d7n;
            else
                return Error_(d7n, "Invalid node.", m);
        }




        public async Task<bool> DeleteFile(int fid)
        {
            var req = _auth.Req.DELETE(_api_file_x, fid);

            Trace_n("Deleting file from server...", "fid: " + fid);
            IResponseShim resp = null; try
            {
                resp = await _client.Send(req);
            }
            catch (Exception ex) { OnUnhandled.Err(this, ex); }

            if (resp == null)
                return Error_(false,
  "Unexpected NULL response.", "IResponseShim".Guillemets());

            if (!resp.IsSuccess)
                return
OnFileDelete.Err(this, (RestServiceException)resp.Error);

            if (resp.Content != "[true]")
                return Error_(false,
"File probably in use by a node.", resp.Content);

            return Trace_n("File successfully deleted from server.", resp.Content);

        }



        //public async Task<bool> Post(FileShim file, int nid)
        //{
        //	//var req = _auth.Req.POST(_api_entity_node_x_attach_file, nid);
        //	var req = _auth.Req.POST(_api_node_x_attach_file, nid);
        //	req.Attachment = file;

        //	Inf("Uploading file to server...", "Attaching to node: " + nid);
        //	IResponseShim resp = null; try
        //	{
        //		resp = await _client.Send(req);
        //	}
        //	catch (Exception ex) { OnUnhandled.Err(this, ex); }
        //	if (resp == null) return false;

        //	if (resp.IsSuccess) return Inf("Returned value:", resp.Content);

        //	return OnNodeAttachFile.Err(this, 
        //		resp.Error as RestServiceException);
        //}


        //public async Task<bool> Put(int nid, Dictionary<string, object> parameters)
        //{
        //	var req = _auth.Req.PUT(_api_entity_node_x, nid);

        //	req.Parameters.Add("node[type]", "app");//hack: hard-code

        //	foreach (var p in parameters)
        //		req.Parameters.Add("node[{0}]".f(p.Key), p.Value);

        //	Trace_n("Updating existing node on server...", "node id: " + nid);

        //	//Warn_n(req.ToString(), "");

        //	IResponseShim resp = null; try
        //	{
        //		resp = await _client.Send(req);
        //	}
        //	catch (Exception ex) { OnUnhandled.Err(this, ex); }
        //	if (resp == null) return false;

        //	if (resp.IsSuccess) return Trace_n("Returned value:", resp.Content);

        //	return OnNodeEdit.Err(this, (RestServiceException)resp.Error);
        //}


        public void SaveSession()
        {
            if (!IsLoggedIn)
            {
                Warn_n("Illogical operation attempted.", "Illogical to save session while disconnected.");
                return;
            }
            SessionAuthFile.Write(_auth.Current, _fsShim, _serialzr);
        }

        public void LoadSession()
        {
            var session = SessionAuthFile.Read(_fsShim, _serialzr);
            if (session == null) Warn_n("Failed to load session.", "Reading SessionAuthFile returned NULL.");
            _auth.Current = session;
        }



    }
}