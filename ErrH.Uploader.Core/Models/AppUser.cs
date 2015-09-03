using System.Collections.Generic;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Uploader.Core.Models
{
    public class AppUser : LoginCredentials
    {
        public List<int> UsedApps { get; set; }
    }
}
