using System;

namespace ErrH.WpfTools.ErrorHandlers
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            throw ex;
        }
    }
}
