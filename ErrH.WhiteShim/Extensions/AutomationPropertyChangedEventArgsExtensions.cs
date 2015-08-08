using System.Windows.Automation;
using ErrH.Tools.Extensions;

namespace ErrH.WhiteShim.Extensions
{
    internal static class AutomationPropertyChangedEventArgsExtensions
{
	internal static string DebugText(this AutomationPropertyChangedEventArgs args, object src)
	{
		var t = "";

		if (src != null)
			t += L.F + "src.ToString: " + src.ToString();

		if (args.EventId != null)
			t += L.f + "args.EventId.Id: " + args.EventId.Id
			   + L.f + "args.EventId.ProgrammaticName: " + args.EventId.ProgrammaticName;

		if (args.Property != null)
			t += L.f + "args.Property.Id: " + args.Property.Id
			   + L.f + "args.Property.ProgrammaticName: " + args.Property.ProgrammaticName;

		if (args.OldValue != null)
			t += L.F + "args.OldValue: " + args.OldValue.ToString();

		if (args.NewValue != null)
			t += L.F + "args.NewValue: " + args.NewValue.ToString();

		return t;
	}
}
}
