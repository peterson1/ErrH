using System;
using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    public class AppFileEventArg : EventArgs
    {
        public List<AppFileDiff>  List  { get; set; }
        public AppFileDiff        File  { get; set; }
        public AppFolder      App   { get; set; }
    }
}
