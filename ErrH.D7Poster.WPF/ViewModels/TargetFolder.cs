using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrH.D7Poster.WPF.ViewModels
{
    public class TargetFolder
    {
        public ObservableCollection<string>      Pending { get; } = new ObservableCollection<string>();
        public ObservableCollection<string>      Done    { get; } = new ObservableCollection<string>();
        public ObservableCollection<Transmittal> OnGoing { get; } = new ObservableCollection<Transmittal>();

        public DirectoryInfo Folder { get; set; }
        public string Label { get; }

        public string FolderPath => Folder.FullName;



        public TargetFolder(string label, string folderPath)
        {
            Label  = label;
            Folder = new DirectoryInfo(folderPath);

            foreach (var file in Folder.EnumerateFiles())
                Pending.Add(file.Name);
        }
    }
}
