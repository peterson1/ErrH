using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleControl;

namespace ErrH.ConsoleCtrlShim
{
internal class WaitFor
{
	internal static async Task<string> Input(ConsoleControl.ConsoleControl cons)
	{
		using (var waitr = new Waiter(cons))
		{
			await waitr.Completion.Task;
			return waitr.Content;			
		}
	}
	
	
	private class Waiter : IDisposable
	{
		private ConsoleControl.ConsoleControl _cons;
		private RichTextBox _rtb;
		private ConsoleEventHandler _consHandlr;
		private KeyPressEventHandler _rtbKeyPress;
		//private KeyEventHandler _rtbKeyDown;

		public TaskCompletionSource<bool> Completion = new TaskCompletionSource<bool>();
		public string Content { get; private set; }

		public Waiter(ConsoleControl.ConsoleControl cons)
		{
			this._cons = cons;
			this._rtb = cons.InternalRichTextBox;
			this._consHandlr = new ConsoleEventHandler(OnConsInput);
			this._rtbKeyPress = new KeyPressEventHandler(OnRtbKeyPress);
			//this._rtbKeyDown = new KeyEventHandler(OnRtbKeyDown);

			this._cons.OnConsoleInput += _consHandlr;
			this._cons.InternalRichTextBox.KeyPress += _rtbKeyPress;
			//this._cons.InternalRichTextBox.KeyDown += _rtbKeyDown;
		}


		//void OnRtbKeyDown(object sender, KeyEventArgs e)
		//{
		//	if (e.KeyCode == Keys.Back)
		//	{
		//		var s = _rtb.Text;
		//		_rtb.Select(s.Length - 1, 1);
		//		_rtb.SelectedText = string.Empty;
		//	}
		//}


		void OnRtbKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter) return;
			var s = e.KeyChar.ToString();
			this.Content += s;
			this._cons.WriteOutput(s, Color.Yellow);
		}

		void OnConsInput(object src, ConsoleControl.ConsoleEventArgs args)
		{
			this.Completion.SetResult(true);
		}

		public void Dispose()
		{
			//this._cons.InternalRichTextBox.KeyDown -= _rtbKeyDown;
			this._cons.InternalRichTextBox.KeyPress -= _rtbKeyPress;
			this._cons.OnConsoleInput -= _consHandlr;
		}
	}

}
}
