using System;

namespace ErrH.Uploader.DataAccess.ErrorHandlers
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            throw ex;
        }
    }
}
