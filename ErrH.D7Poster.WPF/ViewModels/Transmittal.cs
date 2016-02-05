using System.IO;
using PropertyChanged;

namespace ErrH.D7Poster.WPF.ViewModels
{
    [ImplementPropertyChanged]
    public class Transmittal
    {
        public FileInfo File { get; set; }

        public string Name => File.Name;
    }
}
