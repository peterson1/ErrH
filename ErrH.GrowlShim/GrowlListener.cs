using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using Growl.Connector;

namespace ErrH.GrowlShim
{
    public class GrowlListener
    {
        private GrowlConnector _cnn;
        private string         _appName;


        public GrowlListener(string appName)
        {
            _appName = appName;

            _cnn = new GrowlConnector();
            _cnn.Register(new Application(_appName), new NotificationType[]
            {
                Notifier(L4j.Off),
                Notifier(L4j.Fatal),
                Notifier(L4j.Error),
                Notifier(L4j.Warn),
                Notifier(L4j.Info),
                Notifier(L4j.Debug),
                Notifier(L4j.Trace)
            });
        }

        private NotificationType Notifier(L4j level)
        {
            return new NotificationType(level.ToString(), $"Level : ‹{level}›");
        }



        public void HandleLogEvent(object sender, LogEventArg e)
        {
            if (e.Level == L4j.Trace) return;

            if (e.Title.IsBlank()) e.Title = _appName + $"  >>  ‹{sender}›";

            _cnn.Notify(new Notification(
                applicationName  : _appName,
                notificationName : e.Level.ToString(),
                id               : null,
                title            : e.Title,
                text             : e.Message
            ));
        }

    }
}
