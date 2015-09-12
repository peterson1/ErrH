using System.Net;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnFileUpload
{
	public static void Err(ILogSource loggr, RestServiceException ex)
	{
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.InternalServerError:
				msg = "TODO: make this specific";
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg);
	}

}
}
