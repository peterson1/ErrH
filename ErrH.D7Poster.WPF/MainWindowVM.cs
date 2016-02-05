using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ErrH.D7Poster.WPF.ViewModels;

namespace ErrH.D7Poster.WPF
{
    class MainWindowVM
    {
        public ObservableCollection<TargetFolder> Targets { get; } = new ObservableCollection<TargetFolder>();


        public MainWindowVM()
        {
            Targets.Add(new TargetFolder("target dir 1", @"C:\prj\Fats\Fats.Main.Wpf\bin\Debug\logs\pending"));

            SetCurrentItem(Targets[0]);
        }


        private void SetCurrentItem(TargetFolder targetFolder)
        {
            var collectionView = CollectionViewSource.GetDefaultView(Targets);
            collectionView?.MoveCurrentTo(targetFolder);
        }
    }
}
