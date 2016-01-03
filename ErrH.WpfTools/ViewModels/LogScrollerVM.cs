using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.WpfTools.LogFormatters;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public class LogScrollerVM : WorkspaceVmBase
    {
        //private FlowDocument _rtfDoc = new FlowDocument();

        private IFileSystemShim _fs;
        private FileShim        _logFile;
        private ILogSource      _source;
        private ILogFormatter   _rtf = new FlowDocLogFormatter();


        public string  RichText  { get; set; }


        public LogScrollerVM(IFileSystemShim fileSystemShim)
        {
            _fs = fileSystemShim;
        }


        public LogScrollerVM ListenTo(ILogSource logSource, string logFileName = null)
        {
            DisplayName = logSource.GetType().Name;

            _source = logSource;
            //_source.LogAdded += AppendRichText;

            _source.LogAdded += (s, e) => 
            {
                ApppendLogsTo(logFileName, e);
                RichText = _rtf.RewriteToInclude(e);

            };
            return this;
        }


        private void ApppendLogsTo(string logFileName, LogEventArg e)
        {
            if (logFileName.IsBlank()) return;
            switch (e.Level)
            {
                case L4j.Off:               //
                case L4j.Debug:             //  ignore these levels
                case L4j.Trace: return;     //
            }
            if (e.ShowAs != ShowLogAs.Normal) return;

            if (_logFile == null)
                _logFile = _fs.File(_fs.GetAssemblyDir().Bslash(logFileName));

            var line = L.f + TextLog.Format(e.Title, e.Message);
            _logFile.Write(line, EncodeAs.UTF8, false, false);
        }


        //private void AppendRichText(object s, LogEventArg e)
        //{
        //    var p        = new Paragraph();
        //    var r        = new Run(e.Title + "   :   " + e.Message);
        //    r.FontFamily = new FontFamily("Consolas");
        //    r.Foreground = Brushes.White;
        //    r.FontSize   = 10.667;
        //    p.Inlines.Add(r);
        //    _rtfDoc.Blocks.Add(p);

        //    var rnge   = new TextRange(_rtfDoc.ContentStart, _rtfDoc.ContentEnd);
        //    var stream = new MemoryStream();
        //    rnge.Save(stream, DataFormats.Rtf);
        //    RichText   = UTF8Encoding.UTF8.GetString(stream.ToArray());
        //    //RaisePropertyChanged(nameof(RichText));
        //}
    }
}
