using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListViewModel : WorkspaceViewModelBase
    {
        private readonly AppFoldersRepo _repo;


        public AppFolder App { get; }


        public FilesListViewModel(AppFolder appFoldr, AppFoldersRepo repo)
        {
            _repo       = repo;
            App         = appFoldr;
            DisplayName = App.Alias;
        }
    }
}
