using System.Net;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnNodeGet
{
	public static void Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.NotFound:
				msg = "Requested node does not exist on the server.";
				showInnerException = false;
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
	}
}
}
