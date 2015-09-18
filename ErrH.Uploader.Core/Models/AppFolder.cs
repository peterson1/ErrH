using ErrH.Tools.DataAttributes;
using ErrH.Tools.Extensions;
using ErrH.Tools.MvvmPattern;

namespace ErrH.Uploader.Core.Models
{
    public class AppFolder : ListItemVmBase
    {
        [FolderExists]
        public string Path  { get; set; }

        public int    Nid   { get; set; }
        public string Alias { get; set; }


        public override string ToString()
            => (this.Alias.IsBlank()) ? this.Nid.ToString()
                                      : this.Alias;
    }
}
