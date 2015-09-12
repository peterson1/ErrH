using System.Net;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnNodeAdd
{
	public static void Err(ILogSource loggr, RestServiceException ex)
	{
		bool showInnerException = true;
		string msg = ""; switch (ex.Code)
		{
			case HttpStatusCode.NotAcceptable:
				msg = "Invalid JSON object format.";
				showInnerException = false;
				break;

			case HttpStatusCode.Conflict:
				msg = "Value already exists in the database.";
				break;

			case HttpStatusCode.BadRequest:
				msg = "Unrecognized data fields.";
				break;

			case HttpStatusCode.Forbidden:
				var usr = ex.InnerException.Message.Between("Access denied for user ", ").", true);
				msg = "{0} is not allowed to create new nodes.".f(usr.Quotify());
				showInnerException = false;
				break;
		}

		OnAnyEvent.Error(loggr, ex, msg, showInnerException);
	}
}
}
