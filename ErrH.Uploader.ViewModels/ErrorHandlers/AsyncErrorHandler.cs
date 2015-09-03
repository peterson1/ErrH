using System;

namespace ErrH.Uploader.ViewModels.ErrorHandlers
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            throw ex;
        }
    }
}
