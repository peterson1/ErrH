using System.Windows.Controls;

namespace ErrH.WpfTools.UserControls
{
    public partial class BinUpdater : UserControl
    {
        public BinUpdater()
        {
            //DataContextChanged += (s, e) =>
            //{
            //    var src = e.NewValue as ILogSource;
            //    if (src != null)
            //        src.LogAdded += _cons.ShowLog;
            //    else
            //        _cons?.LogHeader(L4j.Warn, "Invalid DataContext type.",
            //            $"Expected ‹{typeof(ILogSource).Name}› but was ‹{DataContext?.GetType().Name}›.");
            //};

            InitializeComponent();
        }
    }
}
