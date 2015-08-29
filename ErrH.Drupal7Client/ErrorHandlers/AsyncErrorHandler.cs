using System;

namespace ErrH.Drupal7Client.ErrorHandlers
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            throw ex;
        }
    }
}
