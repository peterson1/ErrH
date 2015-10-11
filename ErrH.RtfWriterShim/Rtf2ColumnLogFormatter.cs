using System;
using DW.RtfWriter;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using Color = System.Drawing.Color;

namespace ErrH.RtfWriterShim
{
    public class Rtf2ColumnLogFormatter : LogFormatterBase
    {
        const int COL1_WIDTH = 45;

        private RtfDocument     _doc;
        private FontDescriptor  _consolas;
        private ColorDescriptor _fatal;
        private ColorDescriptor _error;
        private ColorDescriptor _warn;
        private ColorDescriptor _info;
        private ColorDescriptor _debug;
        private ColorDescriptor _trace;
        private ColorDescriptor _off;



        public Rtf2ColumnLogFormatter()
        {
            _doc = new RtfDocument(PaperSize.A3,
                                   PaperOrientation.Landscape,
                                   Lcid.English);

            _consolas  = _doc.createFont("Consolas");
        }


        protected override string AppendNormal(L4j level, string title, string message)
        {
            var p = _doc.addParagraph();
            p.DefaultCharFormat.Font = _consolas;
            p.DefaultCharFormat.FontSize = 10.667F;
            p.DefaultCharFormat.FgColor = ColorFor(level);

            p.setText(Concat(title, message));
            return _doc.render();
        }


        protected override string AppendHeader(L4j level, string title, string message)
        {
            throw new NotImplementedException();
        }


        protected override string AppendIntro(L4j level, string text)
        {
            throw new NotImplementedException();
        }


        protected override string AppendOutro(L4j level, string text)
        {
            throw new NotImplementedException();
        }


        private string Concat(string title, string message)
            => title.AlignLeft(COL1_WIDTH, ":") + " " + message;


        private ColorDescriptor ColorFor(L4j level)
        {
            switch (level)
            {
                case L4j.Fatal: return _fatal ?? (_fatal
                    = _doc.createColor(Color.Red));

                case L4j.Error: return _error ?? (_error
                    = _doc.createColor(Color.Red));

                case L4j.Warn: return _warn ?? (_warn
                    = _doc.createColor(Color.Yellow));

                case L4j.Info: return _info ?? (_info
                    = _doc.createColor(Color.White));

                case L4j.Debug: return _debug ?? (_debug
                    = _doc.createColor(Color.DarkGray));

                case L4j.Trace: return _trace ?? (_trace
                    = _doc.createColor(Color.SlateGray));

                case L4j.Off: return _off ?? (_off 
                    = _doc.createColor(Color.Transparent));

                default:
                    throw Error.Unsupported(level);
            }
        }
    }
}
