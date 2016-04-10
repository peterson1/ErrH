using System;

namespace ErrH.Core.PCL45.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler evnt, object sender = null)
            => evnt?.Invoke(sender, EventArgs.Empty);
    }
}
