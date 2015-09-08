using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class CurrentUserBlock : UserControl
    {
        public string Label { get; set; }


        //public string Label
        //{
        //    get { return (string)GetValue(LabelProperty); }
        //    set { SetValue(LabelProperty, value); }
        //}

        //public static readonly DependencyProperty LabelProperty
        //    = DependencyProperty.Register(nameof(Label), typeof(string), typeof(CurrentUserBlock)
        //        , new PropertyMetadata("not logged in"));
        //        //_block.Foreground = Brushes.SeaGreen;




        public CurrentUserBlock()
        {
            InitializeComponent();
        }

    }
}
