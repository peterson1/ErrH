using System.Collections.Generic;

namespace ErrH.Tools.Drupal7Shim.Entities
{
    public class D7User
    {
        public int uid { get; set; }
        public string name { get; set; }
        public string mail { get; set; }

        public Dictionary<int, string> roles { get; set; }
    }
}
