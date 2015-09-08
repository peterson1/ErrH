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


        public string     ExpanderText  { get; set; }
        public UIElement  TopContent    { get; set; }
        public int        InitialWidth  { get; set; }


        public ExpandoTabs()
        {
            Loaded += (s, e) =>
            {
                if (FindTemplateMembers())
                    AddEventHandlers();
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

            _vacantSpace = this.FindChild<Border>(x => x.Name == "_vacantSpace");
            Throw.IfNull(_vacantSpace, "“_vacantSpace” element");

            return true;
        }

        static ExpandoTabs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandoTabs), new FrameworkPropertyMetadata(typeof(ExpandoTabs)));
        }
    }
}
