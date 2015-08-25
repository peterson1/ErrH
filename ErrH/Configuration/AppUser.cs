using System.Collections.Generic;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Configuration
{
    public class AppUser : LoginCredentials
    {
        public List<int> UsedApps { get; set; }
    }
}
