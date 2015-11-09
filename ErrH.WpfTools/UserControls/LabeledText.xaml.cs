using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class LabeledText : UserControl
    {
        public string      Label       { get; set; }
        public string      Text        { get; set; }

        public FontWeight  LabelWeight { get; set; }
        public FontWeight  TextWeight  { get; set; }

        public Brush       LabelBrush  { get; set; }
        public Brush       TextBrush   { get; set; }

        public GridLength  LabelWidth  { get; set; }
        public GridLength  GapWidth    { get; set; }
        public GridLength  TextWidth   { get; set; }



        public LabeledText()
        {
            InitializeComponent();

            //LabelWeight = FontWeights.Normal;
            //TextWeight  = FontWeights.Medium;

            //LabelBrush  = Brushes.Gray;
            //TextBrush   = Brushes.Black;

            //LabelWidth  = new GridLength(3, GridUnitType.Star);
            //GapWidth    = new GridLength(8, GridUnitType.Pixel);
            //TextWidth   = new GridLength(7, GridUnitType.Star);
        }
    }
}
