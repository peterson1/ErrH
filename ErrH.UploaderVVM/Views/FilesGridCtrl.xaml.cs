using System.Windows.Controls;
using ErrH.WpfTools.Extensions;

namespace ErrH.UploaderVVM.Views
{
    public partial class FilesGridCtrl : UserControl
    {
        public FilesGridCtrl()
        {
            InitializeComponent();

            _grid.FixIdleSortGlyph();
        }

    }
}
