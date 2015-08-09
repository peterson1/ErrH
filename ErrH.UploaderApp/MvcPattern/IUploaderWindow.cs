using System;
using System.Collections.Generic;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.MvcPattern
{
    public interface IUploaderWindow
    {
        event EventHandler<AppDirEventArgs> AppSelected;
        event EventHandler<AppFileEventArgs> UploadClicked;
        event EventHandler<AppDirEventArgs> ReplaceLocalsClicked;


        AppDir SelectedApp { get; }
        List<AppFile> ListedFiles { get; }

        void SortFiles(string columnName);
    }
}
