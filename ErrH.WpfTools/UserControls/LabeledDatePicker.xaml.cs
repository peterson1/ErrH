using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class LabeledDatePicker : UserControl
    {
        public string      Label       { get; set; }
        public DateTime?   Date        { get; set; }

        public FontWeight  LabelWeight { get; set; }
        public FontWeight  TextWeight  { get; set; }

        public Brush       LabelBrush  { get; set; }
        public Brush       TextBrush   { get; set; }

        public GridLength  LabelWidth  { get; set; }
        public GridLength  GapWidth    { get; set; }
        public GridLength  TextWidth   { get; set; }

        public bool        IsRequired  { get; set; }




        public LabeledDatePicker()
        {
            InitializeComponent();
        }
    }
}
