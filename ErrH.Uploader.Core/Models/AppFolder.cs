using ErrH.Tools.DataAttributes;
using ErrH.Tools.Extensions;

namespace ErrH.Uploader.Core.Models
{
    public class AppFolder
    {
        public int Nid { get; set; }
        public string Alias { get; set; }

        [FolderExists]
        public string Path { get; set; }


        public override string ToString()
        {
            return (this.Alias.IsBlank()) ? this.Nid.ToString()
                                          : this.Alias;
        }

    }
}
