using System;
using System.Collections.Generic;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class MainWindowViewModel : MainWindowViewModelBase
    {
        private readonly AppFoldersRepo _appFoldersRepo;

        public MainWindowViewModel()
        {
            _appFoldersRepo = new AppFoldersRepo();
        }


        public override List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                CommandViewModel.Relay("View all app folders",
                        x => this.ShowAllFolders()),

                CommandViewModel.Relay("Create new app folder",
                        x => this.CreateNewFolder())
            };
        }

        private void CreateNewFolder()
        {
            var wrkspce = new AppFolderViewModel(new AppFolder(), _appFoldersRepo);
        }

        private void ShowAllFolders()
        {
            throw new NotImplementedException();
        }
    }
}
