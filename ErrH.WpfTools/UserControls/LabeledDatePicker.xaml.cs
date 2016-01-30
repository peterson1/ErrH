using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class LabeledDatePicker : UserControl
    {
        public string      Label       { get; set; }
        public string      Path        { get; set; }

        public FontWeight  LabelWeight { get; set; }
        public FontWeight  TextWeight  { get; set; }

        public Brush       LabelBrush  { get; set; }
        public Brush       TextBrush   { get; set; }

        public GridLength  LabelWidth  { get; set; }
        public GridLength  GapWidth    { get; set; }
        public GridLength  TextWidth   { get; set; }



        public LabeledDatePicker()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                var binding                   = new Binding();
                binding.Source                = DataContext;
                binding.Path                  = new PropertyPath(Path);
                binding.Mode                  = BindingMode.TwoWay;
                binding.UpdateSourceTrigger   = UpdateSourceTrigger.PropertyChanged;
                binding.ValidatesOnDataErrors = true;
                _d8p.SetBinding(DatePicker.SelectedDateProperty, binding);
            };
        }
    }
}
