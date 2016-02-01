using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class LabeledTextBox : UserControl
    {
        public string      Label       { get; set; }
        public string      Path        { get; set; }

        //public bool        CanEdit     { get; set; }

        public FontWeight  LabelWeight { get; set; }
        public FontWeight  TextWeight  { get; set; }

        public Brush       LabelBrush  { get; set; }
        public Brush       TextBrush   { get; set; }

        public GridLength  LabelWidth  { get; set; }
        public GridLength  GapWidth    { get; set; }
        public GridLength  TextWidth   { get; set; }



        public LabeledTextBox()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                _txt.SetBinding(TextBlock.IsEnabledProperty, CanEditBinding());
                _txt.SetBinding(TextBox.TextProperty, PathBinding());
            };
        }


        private BindingBase CanEditBinding()
        {
            var binding    = new Binding();
            binding.Source = DataContext;
            binding.Path   = new PropertyPath("CanAddOrEdit");
            return binding;
        }


        private Binding PathBinding()
        {
            var binding                   = new Binding();
            var isEnabld                  = _txt.IsEnabled;
            binding.Source                = DataContext;
            binding.Path                  = new PropertyPath(Path);
            binding.UpdateSourceTrigger   = UpdateSourceTrigger.LostFocus;
            binding.Mode                  = isEnabld ? BindingMode.TwoWay : BindingMode.OneWay;
            binding.ValidatesOnDataErrors = isEnabld;
            return binding;
        }
    }
}
