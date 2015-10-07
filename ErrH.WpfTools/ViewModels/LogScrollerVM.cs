using System.Windows.Documents;
using System.Windows.Media;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public class LogScrollerVM : WorkspaceVmBase
    {
        private FlowDocument _rtfDoc = new FlowDocument();

        //public Observables<LogListItem>  Events    { get; }
        public string  PlainText { get; set; }
        public string  RichText  { get; set; }



        public LogScrollerVM()
        {
            DisplayName = "Event Logs";
            //Events      = new Observables<LogListItem>();
            //PlainText = new Observables<string>();
        }



        public LogScrollerVM ListenTo(ILogSource logSource)
        {
            PlainText = $"Logging events from {logSource.GetType().Name}...";

            logSource.LogAdded += (s, e) =>
            {
                //Events.Add(new LogListItem(s, e));
                var m = $"[{s.ToString()}]  {e.Message}";
                PlainText += L.f + m;

                AppendRichText(s, e);
            };

            //later: add logging to file

            return this;
        }

        private void AppendRichText(object s, LogEventArg e)
        {
            var rnge = new TextRange(_rtfDoc.ContentEnd, _rtfDoc.ContentEnd);
            var colr = new SolidColorBrush(Color.FromRgb(10, 10, 10));

            rnge.Text = e.Message;
            rnge.ApplyPropertyValue(TextElement.ForegroundProperty, colr);

            //using (var stream = R)
            //{

            //}
            
            //rnge.Save()
        }
    }
}
