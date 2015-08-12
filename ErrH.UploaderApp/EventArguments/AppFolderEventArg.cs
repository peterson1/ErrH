using System;
using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    public class AppFolderEventArg : EventArgs
    {
        public List<AppFolder> List { get; set; }
        public AppFolder App { get; set; }
    }
}
