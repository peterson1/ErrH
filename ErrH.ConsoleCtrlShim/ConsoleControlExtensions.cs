using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErrH.Tools.Extensions;

namespace ErrH.ConsoleCtrlShim
{


public static class ConsoleControlExtensions
{

	public static void Stylize(this ConsoleControl.ConsoleControl cons,
		Color bgColor, string fontFamily = "Consolas", float fontSize = (float)8.25)
	{
		cons.Font = new Font(fontFamily, fontSize);
		cons.BackColor = bgColor;

		cons.InternalRichTextBox.DetectUrls = false;
		cons.InternalRichTextBox.WordWrap = false;
		cons.InternalRichTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
	}


	public static async Task<string> AskFor(this ConsoleControl.ConsoleControl cons,
		string promptMessage)
	{
		cons.Append(L.f + promptMessage.PadLeft(30) + " :  ", Color.Green);
		cons.Focus();

		var content = await WaitFor.Input(cons);

		return content;
	}



	public static void Write2Cols(this ConsoleControl.ConsoleControl cons,
		Color color, string col1, string col2)
	{
		cons.WriteCol1of2 (color, col1);
		cons.WriteLine    (color, col2);
	}


	public static void WriteCol1of2(this ConsoleControl.ConsoleControl cons,
		Color color, string col1, int maxChars = 45)
	{
		col1 = (col1.IsBlank()) ? "" 
			 : col1.AlignLeft(maxChars, " : ");
		
		cons.Append(" " + col1 + " ", color);
	}



	public static void Write3Cols(this ConsoleControl.ConsoleControl cons,
		Color color, string col1, string col2, string col3)
	{
		col1 = col1.AlignRight(13, "...");
		col2 = col2.PadRight(35);

		var fmt = " {0} :  {1} {2}";

		cons.WriteLine(color, fmt, col1, col2, col3);
	}


	public static void TestPalette(this ConsoleControl.ConsoleControl cons)
	{
		cons.TestColor(Color.DimGray);
		cons.TestColor(Color.Gray);
		cons.TestColor(Color.DarkGray);
		cons.TestColor(Color.LightGray);
		cons.BlankLine();

		cons.TestColor(Color.DarkSlateGray);
		cons.TestColor(Color.SlateGray);
		cons.TestColor(Color.LightSlateGray);
		cons.BlankLine();

		cons.TestColor(Color.Yellow);
		cons.TestColor(Color.Red);
	}


	public static void BlankLine(this ConsoleControl.ConsoleControl cons,
		int lineCount = 1)
	{
		cons.Append(L.f.Repeat(lineCount), Color.Black);
	}


	public static void WriteLine(this ConsoleControl.ConsoleControl cons, 
		Color color, string format, params object[] args)
	{
		cons.Append(format.f(args) + L.f, color);
	}


	private static void Append(this ConsoleControl.ConsoleControl cons,
		string text, Color color)
	{
		cons.ScrollToEnd();
		cons.WriteOutput(text, color);
		cons.ScrollToEnd();
	}


	public static void ScrollToEnd(this ConsoleControl.ConsoleControl cons)
	{
		var rtb = cons.InternalRichTextBox;
		rtb.SelectionStart = rtb.Text.Length; 
		rtb.ScrollToCaret();
	}


	public static void WriteWhite(this ConsoleControl.ConsoleControl cons,
		string format, params object[] args)
	{
		cons.WriteLine(Color.White, format, args);
	}


	private static void TestColor(this ConsoleControl.ConsoleControl cons, 
		Color color)
	{
		cons.WriteLine(color, "Sphinx of black quartz, judge my vow. : " + color);
	}


}}
