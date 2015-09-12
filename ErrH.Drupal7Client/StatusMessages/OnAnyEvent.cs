using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnAnyEvent
{
	public static void Error(ILogSource loggr, 
							 RestServiceException ex, 
							 string msg,
							 bool showInnerException = true)
	{
		if (msg.IsBlank())
			msg = "Http Error {0} :  {1}".f((int)ex.Code, ex.Code);

		//var line1 = loggr.Title;
		var line2 = showInnerException ? ex.Details(false, false) 
									   : "\t" + ex.Message;
		//loggr.Title = line1;
		loggr.Error_n("Error encountered", msg + L.F + line2);
	}
}
}
