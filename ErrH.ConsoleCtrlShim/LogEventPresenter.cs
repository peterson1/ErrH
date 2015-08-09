using System.Drawing;
using System.Runtime.CompilerServices;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.ConsoleCtrlShim
{
public static class LogEventPresenter
{
	private static ConditionalWeakTable<LogEventArg, object> _argsFired = new ConditionalWeakTable<LogEventArg, object>();



	public static T CatchLogs<T>(this ConsoleControl.ConsoleControl cons,
		T logEventRaiser) where T : ILogSource
	{
		logEventRaiser.LogAdded += (object src, LogEventArg e) => 
		{
			//object o;
			//if (_argsFired.TryGetValue(e, out o))
			//	_argsFired.Remove(e);
			//else
			//{
			//	_argsFired.Add(e, null);
			//	TwoColumnLayout(cons, e);
			//}

			if (!Fired(e)) TwoColumnLayout(cons, e);

		};

		return logEventRaiser;
	}



	private static bool Fired(LogEventArg e, object o = null)
	{
		if (_argsFired.TryGetValue(e, out o)) return true;		
		_argsFired.Add(e, null);
		return false;
	}


	private static void TwoColumnLayout(ConsoleControl.ConsoleControl cons, LogEventArg e)
	{
		var colr = GetColor(e.Level);

		if (e.Title == No.Break || e.Message == No.Break)
		{
			if (e.ShowAs == ShowLogAs.Intro)
				cons.WriteCol1of2(colr, e.Title);

			else if (e.ShowAs == ShowLogAs.Outro)
				cons.WriteLine(colr, e.Message);

			else
				Throw.Unsupported(e.ShowAs, No.Break);
		}
		else
		{
			if (e.Level == L4j.Info && e.Title.EndsWith("..."))
				cons.BlankLine();

			cons.Write2Cols(colr, e.Title, e.Message);
		}


	}


	public static void TestLevels(this ConsoleControl.ConsoleControl cons)
	{
		cons.TestLevel(L4j.Info);
		cons.TestLevel(L4j.Debug);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Warn);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Error);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Info);
		cons.BlankLine();
		
		cons.TestLevel(L4j.Info);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Warn);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Info);
		cons.BlankLine();
		
		cons.TestLevel(L4j.Info);
		cons.TestLevel(L4j.Debug);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Warn);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Trace);
		cons.TestLevel(L4j.Info);
		cons.BlankLine();

		cons.TestLevel(L4j.Info);
		cons.TestLevel(L4j.Error);
		cons.TestLevel(L4j.Fatal);
	}

	private static void TestLevel(this ConsoleControl.ConsoleControl cons,
		L4j level)
	{
		cons.Write3Cols(GetColor(level), 
				 "SomeObjectAsSouce", 
				 "Testing [{0}] level".f(level), 
				 "This is a sampler of level {0}.".f(level));
	}


	private static Color GetColor(L4j level)
	{
		switch (level)
		{
			case L4j.Fatal: return Color.Red;
			case L4j.Error: return Color.Red;
			case L4j.Warn:  return Color.Yellow;
			//case L4j.Info:  return Color.LightGray;
			//case L4j.Debug: return Color.SlateGray;
			//case L4j.Trace: return Color.DarkSlateGray;
			case L4j.Info:  return Color.White;
			case L4j.Debug: return Color.DarkGray;
			case L4j.Trace: return Color.SlateGray;
			default:
				throw Error.Unsupported(level, "color map");
		}
	}

}
}
