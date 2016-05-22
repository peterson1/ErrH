using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoDependencyPropertyMarker;

namespace ErrH.Wpf.net45.UserControls
{
    [AutoDependencyProperty]
    public partial class LabeledText : UserControl
    {
        public string      Label           { get; set; }
        public string      Text            { get; set; }
                                           
        public FontWeight  LabelWeight     { get; set; }
        public FontWeight  TextWeight      { get; set; }
                                           
        public Brush       LabelBrush      { get; set; }// unused?
        public Brush       TextBrush       { get; set; }//
                                           
        public double?     LabelOpacity    { get; set; }
        public double?     TextOpacity     { get; set; }
                                           
        public GridLength  LabelWidth      { get; set; }
        public GridLength  GapWidth        { get; set; }
        public GridLength  TextWidth       { get; set; }
                                           
        public FontStyle   LabelFontStyle  { get; set; }
        public FontStyle   TextFontStyle   { get; set; }


        public LabeledText()
        {
            //if (!LabelOpacity.HasValue) LabelOpacity = 0.5;
            //if (! TextOpacity.HasValue)  TextOpacity = 1;

            InitializeComponent();
            //if (LabelOpacity == 0) LabelOpacity = 0.5;
        }
    }
}
