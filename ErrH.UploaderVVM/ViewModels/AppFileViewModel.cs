using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.UploaderVVM.ViewModels
{
    [ImplementPropertyChanged]
    public class AppFileViewModel : ViewModelBase
    {
        public AppFileNode Model { get; }

        public string Name => Model?.Name;


        public override string DisplayName => Model?.Name;



        public AppFileViewModel(AppFileNode appFile)
        {
            Model = appFile;
        }
    }
}
