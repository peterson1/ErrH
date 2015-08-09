using ErrH.Tools.Extensions;

namespace ErrH.UploaderApp.Models
{
    public class AppDir
    {
        public int     Nid    { get; set; }
        public string  Alias  { get; set; }
        public string  Path   { get; set; }


        public override string ToString()
        {
            return (this.Alias.IsBlank()) ? this.Nid.ToString()
                                          : this.Alias;
        }

    }

}
