using System.Net;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnFileDelete
{
	public static bool Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{			
			case HttpStatusCode.Unused:
				var usr = ex.InnerException.Message.Between("Access denied for user ", ").", true);
				msg = "{0} is not allowed to delete this file.".f(usr.Quotify());
				showInnerException = false;
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
		return false;
	}
}
}
