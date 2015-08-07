using System;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7Client.StatusMessages
{
    public class OnUnhandled
{
	public static void Err(ILogSource loggr, Exception ex)
	{
		loggr.Error_n("Unhandled server error.", "");
		loggr.Error_n(L.F + ex.Message(), "");
		loggr.Error_n(ex.ToString(), "");
	}
}
}
