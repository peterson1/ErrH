using System;
using System.Threading.Tasks;
using ErrH.Drupal7Client;
using ErrH.Tools.Drupal7Models;
using ErrH.UploaderApp.AppFileRepository;

namespace ErrH.UploaderApp.Repositories
{
    public class RemoteFilesRepo : D7NodesRepoBase<AppFileRepo_Dto, AppFileNode>
    {

        public RemoteFilesRepo(ID7Client d7Client) 
            : base(d7Client) { }


        protected override Func<AppFileNode, object> 
            GetKey => x => x.Name;



        protected override AppFileNode FromDto(AppFileRepo_Dto dto)
            => AppFileNode.FromDto(dto, null);

    }
}
