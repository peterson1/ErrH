using System;
using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using ErrH.ConsoleCtrlShim;
using ErrH.Tools.Loggers;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.CustomControls
{
    [TemplatePart(Name="_cons", Type=typeof(TwoColumnLogCtrl))]
    [TemplatePart(Name="_expander", Type=typeof(Expander))]
    [TemplatePart(Name="_grid", Type=typeof(Grid))]
    [AutoDependencyProperty]
    public class TabsAndConsole : TabControl
    {
        private TwoColumnLogCtrl _cons;
        private Expander         _expander;
        private Grid             _grid;
        private GridLength       _previousHeight = GridLength.Auto;


        public string     ExpanderHeader  { get; set; }
        public UIElement  CornerContent   { get; set; }




        public TabsAndConsole()
        {
            Loaded += (s, e) =>
            {
                if (FindTemplateMembers())
                    AssignEventHandlers();
            };
        }


        public void ShowLog(object s, LogEventArg e)
            => _cons.ShowLog(s, e);



        private void AssignEventHandlers()
        {
            _expander.Collapsed += (s, e) =>
            {
                _previousHeight = Row2.Height;
                Row2.Height = GridLength.Auto;
            };

            _expander.Expanded += (s, e) =>
            {
                Row2.Height = _previousHeight;
            };
        }


        private bool FindTemplateMembers()
        {
            try {
                _cons     = this.Find<TwoColumnLogCtrl>("_cons");
                _expander = this.Find<Expander>("_expander");
                _grid     = this.Find<Grid>("_grid");
                return true;

            }
            catch (Exception )
            {
                //MessageBox.Show(ex.Details(), "Missing template member.");
                return false;
            }
        }


        private RowDefinition Row2 => _grid.RowDefinitions[2];


        static TabsAndConsole()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabsAndConsole), 
                new FrameworkPropertyMetadata(typeof(TabsAndConsole)));
        }
    }
}
