using System;
using ErrH.BinUpdater.Core.DTOs;
using ErrH.BinUpdater.Core.Models;
using ErrH.Tools.Authentication;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.BinUpdater.Core.Configuration
{
    public interface IConfigFile : IRemoteSettings, ILogSource
    {
        /// <summary>
        /// Parses file and loads values into the instance.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>Returns 'False' if no file found.</returns>
        bool ReadFrom<T>(string folderPath) where T : ConfigFileDto;


        event EventHandler<UrlEventArg> CertSelfSigned;
        event EventHandler<EArg<LoginCredentials>> CredentialsReady;


        bool Exists { get; }
        string FixedName { get; }
        string Path { get; }


        AppUser AppUser { get; }


        /// <summary>
        /// Writes all values to file.
        /// </summary>
        void Save();
    }
}
