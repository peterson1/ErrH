using System;
using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    public class AppDirEventArgs : EventArgs
    {
        public List<AppDir> List { get; set; }
        public AppDir App { get; set; }
    }
}
