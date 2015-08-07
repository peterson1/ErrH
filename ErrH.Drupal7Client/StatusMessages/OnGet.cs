using System.Net;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnGet
{
	public static bool Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.NotFound:
				msg = "Resource not found:  " + ex.BaseUrl.Slash(ex.Resource);
				showInnerException = false;
				break;			
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
		return false;
	}
}
}
