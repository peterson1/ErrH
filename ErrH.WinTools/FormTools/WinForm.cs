using System;
using System.Windows.Forms;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;

namespace ErrH.WinTools.FormTools
{
    public class WinForm
    {
        public static ILifetimeScopeShim Shim(ITypeResolver resolvr)
        {
            Application.ThreadException
                += (s, e) => { HandleErr(e.Exception); };

            AppDomain.CurrentDomain.UnhandledException
                += (s, e) => { HandleErr(e.ExceptionObject); };


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            return resolvr.BeginLifetimeScope();
        }


        static void HandleErr(object exceptionObj)
        {
            var ex = exceptionObj as Exception;
            if (ex == null)
            {
                HandleErr(new Exception(
                    "Non-exception object thrown: " + exceptionObj.GetType().Name));
                return;
            }
            MessageBox.Show(ex.Message(true, false), "Unhandled Exception");
        }

    }
}
