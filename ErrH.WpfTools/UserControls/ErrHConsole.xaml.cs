using System.Windows.Controls;
using System.Windows.Media;
using ErrH.Tools.Extensions;
using ErrH.ConsoleCtrlShim;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.UserControls
{
    /// <summary>
    /// Interaction logic for ErrHConsole.xaml
    /// </summary>
    public partial class ErrHConsole : UserControl
    {
        private TwoColumnLogCtrl _cons;

        public ErrHConsole()
        {
            InitializeComponent();

            this.Loaded += (s, e) => { _cons 
                = (TwoColumnLogCtrl)_host.Child; };
        }



        public void LogNormal(L4j level, string col1, string col2)
            => _cons.LogNormal(level, col1, col2);



        public string Header
        {
            get { return (string)_expander.Header; }
            set { _expander.Header = value; }
        }


        //public Expander Expander => _expander;
    }
}
