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
                if (FindTemplateMembers())
                    AddEventHandlers();

                //if (this.TryFindChild<GridSplitter>(x => x.Name == "_splitter", out _splitter))
                //    _splitter.LostMouseCapture += (s, e) =>
                //    {
                //        MessageBox.Show("LostMouseCapture");
                //    };
            };
        }



        private void AddEventHandlers()
        {
            _vacantSpace.MouseDown += (s, e) =>
            {
                _expander.IsExpanded = !_expander.IsExpanded;
            };
        }



        private bool FindTemplateMembers()
        {
            _expander = this.Find<Expander>("_expander");
            Throw.IfNull(_expander, "_expander element");

            _vacantSpace = this.FindChild<Border>(x => x.Name == "_vacantSpace");

            return true;
        }

        static ExpandoTabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandoTabs), new FrameworkPropertyMetadata(typeof(ExpandoTabs)));
        }
    }
}
