using System;
using System.Collections.Generic;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.MvcPattern
{
    public interface IUploaderWindow
    {
        event EventHandler<AppFolderEventArg> AppSelected;
        event EventHandler<AppFileEventArg> UploadClicked;
        event EventHandler<AppFolderEventArg> ReplaceLocalsClicked;


        AppFolder SelectedApp { get; }
        List<AppFileDiffs> ListedFiles { get; }

        void SortFiles(string columnName);
    }
}
