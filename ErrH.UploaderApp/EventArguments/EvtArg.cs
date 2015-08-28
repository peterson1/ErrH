using System.Collections.Generic;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.EventArguments
{
    public class EvtArg
    {




        public static AppFolderEventArg AppDir(List<AppFolder> list)
        {
            return new AppFolderEventArg { List = list };
        }

        public static AppFolderEventArg AppDir(AppFolder app)
        {
            return new AppFolderEventArg { App = app };
        }

    }
}
