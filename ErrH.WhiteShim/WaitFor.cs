using System;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ErrH.WhiteShim
{
internal class WaitFor
{
	internal static async Task<AutomationEventArgs> Event(
		AutomationEvent eventId, 
		AutomationElement element = null, 
		TreeScope scope = TreeScope.Descendants)
	{
		using (var waitr = new Waiter(eventId, element, scope))
		{
			await waitr.Completion.Task;
			return waitr.EvtArgs;
		}
	}


	private class Waiter : IDisposable
	{
		public TaskCompletionSource<bool> Completion = new TaskCompletionSource<bool>();
		
		public AutomationEventArgs EvtArgs { get; private set; }

		private AutomationEvent        _eventId;
		private AutomationElement      _element;
		private TreeScope              _scope;
		private AutomationEventHandler _handler;


		internal Waiter(AutomationEvent eventId, 
						AutomationElement element = null, 
						TreeScope scope = TreeScope.Descendants)
		{
			this._eventId = eventId;
			this._element = element ?? AutomationElement.RootElement;
			this._scope   = scope;
			this._handler = new AutomationEventHandler((src, e) => 
			{
				this.EvtArgs = e;
				this.Completion.SetResult(true);
			});

			Automation.AddAutomationEventHandler(
				this._eventId, this._element,
				this._scope,   this._handler);
		}


		public void Dispose()
		{
			Automation.RemoveAutomationEventHandler(
				_eventId, _element, _handler);
		}


	}
}
}
