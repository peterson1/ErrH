using System;
using System.Threading;
using System.Windows;
using ErrH.Tools.Extensions;
using ErrH.WinTools.ReflectionTools;

namespace ErrH.Uploader.ViewModels.ErrorHandlers
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            //var msg = $"Error on async method: {MethodNow.JumpBack(4).Name}()";
            //throw new ThreadInterruptedException(msg, ex);
            throw ex;
            //MessageBox.Show(ex.Details(true, true), "Async method error");
        }
    }
}
