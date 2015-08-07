using System.Net;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnLogin
{
	public static void Err(ILogSource loggr, RestServiceException ex)
	{
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.Forbidden:
				msg = "SSL certificate is invalid.";
				break;
				
			case HttpStatusCode.ServiceUnavailable:
				msg = "Unable to reach the server."; 
				break;
			
			case HttpStatusCode.Unauthorized:
				msg = "Server rejected application credentials.";
				break;
		}
		
		OnAnyEvent.Error(loggr, ex, msg);
	}


}
}
