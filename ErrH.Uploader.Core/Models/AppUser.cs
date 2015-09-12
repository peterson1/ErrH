using System.Collections.Generic;
using ErrH.Tools.Authentication;

namespace ErrH.Uploader.Core.Models
{
    public class AppUser : LoginCredentials
    {
        public List<int> UsedApps { get; set; }
    }
}
