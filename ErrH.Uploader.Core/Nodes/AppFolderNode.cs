using System.Collections.Generic;
using System.Collections.ObjectModel;
using ErrH.Tools.Drupal7Models;

namespace ErrH.Uploader.Core.Nodes
{
    public class AppFolderNode
    {
        //public AppFileRepo Repo { get; set; }
        public ID7Repo Repo { get; set; }

        public int Nid { get; set; }
        public int Vid { get; set; }
        public string Title { get; set; }
        public List<int> Users { get; set; }

        public ReadOnlyCollection<AppFileNode> Files
        {
            get
            {
                //todo: what's this for?
                //return Files();
                return null;
            }
        }
    }
}
