using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using ErrH.Tools.ErrorConstructors;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.CustomControls
{

    [AutoDependencyProperty]
    public class ExpandoTabs : TabControl
    {
        private Expander _expander;
        private Border _vacantSpace;
        //private GridSplitter _splitter;


        public string           ExpanderText     { get; set; }
        public ExpandDirection  ExpandDirection  { get; set; }
        public int              InitialWidth     { get; set; }
        public int              InitialHeight    { get; set; }
        public UIElement        TopContent       { get; set; }


        public ExpandoTabs()
        {
            Loaded += (src, ea) =>
            {
                FindTemplateMembers();
            };
        }





        private void FindTemplateMembers()
        {
            _expander = this.Find<Expander>("_expander");
            Throw.IfNull(_expander, "_expander element");

            _vacantSpace = this.Find<Border>(name: "_vacantSpace");
            //if (this.TryFindChild<Border>(x=>x.Name == "_vacantSpace", out _vacantSpace))
                _vacantSpace.MouseDown += (s, e) =>
                {
                    _expander.IsExpanded = !_expander.IsExpanded;
                };
        }

        static ExpandoTabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandoTabs), new FrameworkPropertyMetadata(typeof(ExpandoTabs)));
        }
    }
}
