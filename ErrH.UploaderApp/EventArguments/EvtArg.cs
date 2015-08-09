using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    internal class EvtArg
    {

        public static AppFileEventArgs AppFile(AppDir app, List<AppFile> list)
        {
            return new AppFileEventArgs { App = app, List = list };
        }


        public static AppFileEventArgs AppFile(List<AppFile> list)
        {
            return new AppFileEventArgs { List = list };
        }


        public static AppFileEventArgs AppFile(AppFile file)
        {
            return new AppFileEventArgs { File = file };
        }





        public static AppDirEventArgs AppDir(List<AppDir> list)
        {
            return new AppDirEventArgs { List = list };
        }

        public static AppDirEventArgs AppDir(AppDir app)
        {
            return new AppDirEventArgs { App = app };
        }

    }
}
