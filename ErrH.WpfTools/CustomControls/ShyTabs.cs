using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.CustomControls
{
    [AutoDependencyProperty]
    public class ShyTabs : ErrTabs
    {
        private Expander _expander;
        private Border   _vacantSpace;


        public string           ExpanderText     { get; set; }
        public ExpandDirection  ExpandDirection  { get; set; }
        public int              InitialWidth     { get; set; }
        public int              InitialHeight    { get; set; }
        public UIElement        TopContent       { get; set; }
        public bool             IsCollapsed      { get; set; }



        public ShyTabs()
        {
            Loaded += (s, e) =>
            {
                _expander    = this.Find<Expander>("_expander");
                _vacantSpace = this.Find<Border>(name: "_vacantSpace");
                _vacantSpace.MouseDown += (t, f) 
                    => { _expander.Toggle(); };
            };
        }



        static ShyTabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShyTabs), new FrameworkPropertyMetadata(typeof(ShyTabs)));
        }
    }
}
