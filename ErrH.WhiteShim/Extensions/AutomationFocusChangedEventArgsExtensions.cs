using System.Windows.Automation;
using ErrH.Tools.Extensions;

namespace ErrH.WhiteShim.Extensions
{
    internal static class AutomationFocusChangedEventArgsExtensions
{
	internal static string DebugText(this AutomationFocusChangedEventArgs args, object src)
	{
		var t = "";

		if (src != null)
			t += L.F + "src.ToString: " + src.ToString();

		t += L.f + "args.ChildId: " + args.ChildId
		   + L.f + "args.ObjectId: " + args.ObjectId;

		if (args.EventId != null)
			t += L.f + "args.EventId.Id: " + args.EventId.Id
			   + L.f + "args.EventId.ProgrammaticName: " + args.EventId.ProgrammaticName;

		return t;
	}
}
}
