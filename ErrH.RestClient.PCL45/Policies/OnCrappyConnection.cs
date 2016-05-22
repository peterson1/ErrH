using System;
using System.Net;
using System.Threading;
using Polly;

namespace ErrH.RestClient.PCL45.Policies
{
    public class OnCrappyConnection
    {
        public static Policy RetryForever(int retryDelaySeconds, Action<string> errorNotifier = null)
            => Policy.Handle<WebException>(x => IsCrappy(x)).RetryForeverAsync(ex =>
            {
                errorNotifier?.Invoke(FormatError(ex, retryDelaySeconds));

                using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                {
                    tmpEvent.WaitOne(TimeSpan.FromSeconds(retryDelaySeconds));
                }
            });


        private static string FormatError<T>(T ex, int retryDelaySeconds) where T : Exception
        {
            var wx = ex as WebException;
            //if (wx == null) return $"‹{typeof(T).Name}› “{ex.Message}”";
            //return $"[{wx.Status}] “{ex.Message}”";

            var prefx = wx == null ? $"‹{typeof(T).Name}›" : $"[{wx.Status}]";
            return $"{prefx} “{ex.Message}”  :  Retrying in {retryDelaySeconds} seconds ...";
        }


        private static bool IsCrappy(WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ConnectFailure) return true;
            if (ex.Status == WebExceptionStatus.SendFailure) return true;
            if (ex.Message.ToLower().Contains("kept alive")) return true;
            if (ex.Message.ToLower().Contains("timed out")) return true;
            if (ex.Message.ToLower().Contains("connection was closed")) return true;
            if (ex.Message.ToLower().Contains("deadlock")) return true;
            return false;
        }
    }
}
