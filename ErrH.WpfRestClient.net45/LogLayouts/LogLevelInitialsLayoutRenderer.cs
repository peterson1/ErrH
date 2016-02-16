using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace ErrH.WpfRestClient.net45.LogLayouts
{
    [LayoutRenderer("level-short")]
    public class LogLevelInitialsLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
            => builder.Append(logEvent.Level.Name[0]);
    }
}
