using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ErrH.Tools.Loggers;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public class LogScrollerVM : WorkspaceVmBase
    {
        private FlowDocument _rtfDoc = new FlowDocument();

        private ILogFormatter _rtf;
        private ILogSource    _source;

        public string  RichText  { get; set; }



        //public LogScrollerVM(ILogFormatter logFormatter)
        //{
        //    _format = logFormatter;
        //}


        public LogScrollerVM ListenTo(ILogSource logSource)
        {
            _source = logSource;
            //_source.LogAdded += AppendRichText;

            _source.LogAdded += (s, e) 
                => { RichText = _rtf.RewriteToInclude(e); };
            
            //later: add logging to file

            return this;
        }

        private void AppendRichText(object s, LogEventArg e)
        {
            var p        = new Paragraph();
            var r        = new Run(e.Title + "   :   " + e.Message);
            r.FontFamily = new FontFamily("Consolas");
            r.Foreground = Brushes.White;
            r.FontSize   = 10.667;
            p.Inlines.Add(r);
            _rtfDoc.Blocks.Add(p);

            var rnge   = new TextRange(_rtfDoc.ContentStart, _rtfDoc.ContentEnd);
            var stream = new MemoryStream();
            rnge.Save(stream, DataFormats.Rtf);
            RichText   = UTF8Encoding.UTF8.GetString(stream.ToArray());
            //RaisePropertyChanged(nameof(RichText));
        }
    }
}
