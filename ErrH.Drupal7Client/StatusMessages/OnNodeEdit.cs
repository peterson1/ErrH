using System.Net;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnNodeEdit
{
	public static bool Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.NotAcceptable:
				msg = "Invalid JSON object format.";
				showInnerException = false;
				break;
			
			case HttpStatusCode.Forbidden:
				var usr = ex.InnerException.Message.Between("Access denied for user ", ").", true);
				msg = "{0} is not allowed to edit this node.".f(usr.Quotify());
				showInnerException = false;
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
		return false;
	}
}
}
