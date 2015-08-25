using System;
using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    public class AppFileEventArg : EventArgs
    {
        public List<AppFileDiffs>  List  { get; set; }
        public AppFileDiffs        File  { get; set; }
        public AppFolder      App   { get; set; }
    }
}
