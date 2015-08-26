using System.Windows.Controls;
using ErrH.WpfTools.Extensions;

namespace ErrH.UploaderVVM.Views
{
    /// <summary>
    /// Interaction logic for FilesGridCtrl.xaml
    /// </summary>
    public partial class FilesGridCtrl : UserControl
    {
        public FilesGridCtrl()
        {
            InitializeComponent();

            _grid.UpdateSortGlyph();
        }

    }
}
