using System.IO;
using System.Text;
using System.Windows;
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
        public string  RichText  { get; set; } = "";



        public LogScrollerVM()
        {
            DisplayName = "Event Logs";
            //Events      = new Observables<LogListItem>();
            //PlainText = new Observables<string>();

            //_rtfDoc.FontFamily = new FontFamily("Consolas");
            //_rtfDoc.FontSize = 10.667;

            //var pStyle = new Style { TargetType = typeof(Paragraph) };
            //pStyle.Setters.Add(new Setter {
            //    Property = Paragraph.MarginProperty,
            //    Value = new Thickness(0)
            //});
            //_rtfDoc.Resources.Add(null, pStyle);
        }



        public LogScrollerVM ListenTo(ILogSource logSource)
        {
            PlainText = $"Logging events from {logSource.GetType().Name}...";

            //for (int i = 0; i < 100; i++)
            //{
            //    AppendRichText(null, new LogEventArg { Message = $"{i}" });
            //}

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
            //_rtfDoc.Blocks.ForEach(x => x.As<Paragraph>().Margin = new Thickness(0));
            //_rtfDoc.Blocks.ForEach(x => x.As<Paragraph>().LineHeight = 10);

            if (e.Message.IsBlank()) return;

            //var rnge = new TextRange(_rtfDoc.ContentEnd, _rtfDoc.ContentEnd);
            //rnge.Text = L.f + e.Message;
            //var colr = new SolidColorBrush(Color.FromRgb(255,255,255));
            //rnge.ApplyPropertyValue(TextElement.ForegroundProperty, colr);
            //rnge.ApplyPropertyValue(Paragraph.MarginProperty, new Thickness(0));

            //var stream = new MemoryStream();
            //rnge.Save(stream, DataFormats.Rtf);
            //var text = UTF8Encoding.UTF8.GetString(stream.ToArray());

            //RichText += text;

            var p = new Paragraph();
            //p.Margin = new Thickness(0);
            var r = new Run(e.Message);
            r.FontFamily = new FontFamily("Consolas");
            r.Foreground = Brushes.White;
            r.FontSize = 10.667;
            p.Inlines.Add(r);
            _rtfDoc.Blocks.Add(p);

            var rnge = new TextRange(_rtfDoc.ContentStart, _rtfDoc.ContentEnd);
            var stream = new MemoryStream();
            rnge.Save(stream, DataFormats.Rtf);
            var text = UTF8Encoding.UTF8.GetString(stream.ToArray());

            RichText = text;

            //RaisePropertyChanged(nameof(RichText));
        }
    }
}
