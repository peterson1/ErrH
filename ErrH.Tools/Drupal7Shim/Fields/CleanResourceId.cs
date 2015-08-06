using System.Collections.Generic;
using System.Linq;

namespace ErrH.Tools.Drupal7Shim.Fields
{
    public class CleanResourceId
    {
        public int id { get; set; }
        public string resource { get; set; }
        //public string uri { get; set; }

        public CleanResourceId() { }
        public CleanResourceId(string resource, int id)
        {
            this.resource = resource;
            this.id = id;
        }


        public static CleanResourceId File(int fid)
        {
            return new CleanResourceId("file", fid);
        }

        public static CleanResourceId User(int uid)
        {
            return new CleanResourceId("user", uid);
        }

        public static CleanResourceId Node(int nid)
        {
            return new CleanResourceId("node", nid);
        }



        public static List<CleanResourceId> Users(params int[] uids)
        {
            return uids.Select(x => CleanResourceId.User(x)).ToList();
        }

        public static List<CleanResourceId> Files(params int[] fids)
        {
            return fids.Select(x => CleanResourceId.File(x)).ToList();
        }

    }
}
