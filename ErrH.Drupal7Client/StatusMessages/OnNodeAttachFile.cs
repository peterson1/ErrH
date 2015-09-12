using System.Net;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnNodeAttachFile
{
	public static bool Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.BadRequest:
				msg = "Invalid field values.";
				showInnerException = false;
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
		return false;
	}
}
}
