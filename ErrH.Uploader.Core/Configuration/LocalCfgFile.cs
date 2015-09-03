using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.Serialization;
using ErrH.Uploader.Core.DTOs;
using ErrH.Uploader.Core.Models;
using static ErrH.Tools.ScalarEventArgs.EArg<ErrH.Tools.RestServiceShim.LoginCredentials>;

namespace ErrH.Uploader.Core.Configuration
{
    public class LocalCfgFile : LogSourceBase, IConfigFile
    {
        private EventHandler<UrlEventArg> _certSelfSigned;
        public event EventHandler<UrlEventArg> CertSelfSigned
        {
            add { _certSelfSigned -= value; _certSelfSigned += value; }
            remove { _certSelfSigned -= value; }
        }

        private EventHandler<EArg<LoginCredentials>> _credentialsReady;
        public event EventHandler<EArg<LoginCredentials>> CredentialsReady
        {
            add { _credentialsReady -= value; _credentialsReady += value; }
            remove { _credentialsReady -= value; }
        }

        protected ConfigFileDto _dto;
        protected IFileSystemShim _fs;
        protected FileShim _file;
        private ISerializer _serialr;
        private bool _appNotifiedOfInvalidSSL = false;


        public LocalCfgFile(IFileSystemShim fsShim, ISerializer serializer)
        {
            _fs = ForwardLogs(fsShim);
            _serialr = ForwardLogs(serializer);
        }


        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int AppNid { get; set; }
        public bool ValidSSL { get; set; }
        public int Interval { get; set; }

        public string FixedName { get { return ".errh"; } }
        public bool Exists { get { return _file.Found; } }
        public string Path { get { return _file.Path; } }



        public bool ReadFrom<T>(string folderPath) where T : ConfigFileDto
        {
            Info_n("Reading settings from config file...", "");

            var dir = _fs.Folder(folderPath);
            if (!dir.Found) return false;

            this._file = dir.File(this.FixedName, false);
            if (!_file.Found) return false;

            var content = _file.ReadUTF8;
            _dto = _serialr.Read<T>(content);
            if (_dto == null) return false;

            //if (!this.IsValid(_dto)) return false;
            //var err = DataError.Info(_dto);
            var err = DataError.Info(_dto as T);
            if (!err.IsBlank())
                return Warn_n("Validation failed.", err);

            this.Server = _dto.base_url;
            this.Username = _dto.username;
            this.Password = _dto.password;
            this.AppNid = _dto.app_nid;
            this.ValidSSL = _dto.valid_ssl;
            this.Interval = _dto.mins_interval;


            if (!this.ValidSSL && !_appNotifiedOfInvalidSSL)
            {
                Warn_n("Server is using a self-signed certificate.",
                       "Application must be set to allow SSL from the server.");
                _certSelfSigned?.Invoke(this, EventArg.Url(this.Server));
                _appNotifiedOfInvalidSSL = true;
            }


            Info_n("Loaded settings from config file.", "");

            _credentialsReady?.Invoke(this, NewArg(AppUser));

            return true;
        }



        public AppUser AppUser
        {
            get
            {
                return new AppUser
                {
                    Name = this.Username,
                    Password = this.Password,
                    BaseUrl = this.Server,
                    ValidSSL = this.ValidSSL,
                    RetryIntervalSeconds = this.Interval * 60,
                    UsedApps = new List<int> { this.AppNid }
                };
            }
        }



        public void Save()
        {
            Info_n("Writing settings to config file...", this.Path);

            var dto = new ConfigFileDto
            {
                base_url = this.Server,
                username = this.Username,
                password = this.Password,
                app_nid = this.AppNid,
                valid_ssl = this.ValidSSL,
                mins_interval = this.Interval,
            };

            _file.Write(_serialr.Write(dto, true));
            _file.Hidden = true;

            Info_n("Settings successfully saved.", "");
        }
    }
}
