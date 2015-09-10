using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.CustomControls
{
    [AutoDependencyProperty]
    public class ErrTabs : TabControl
    {

        public UIElement CornerContent { get; set; }



        static ErrTabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrTabs), new FrameworkPropertyMetadata(typeof(ErrTabs)));
        }
    }
}
