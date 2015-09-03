using System;
using ErrH.Drupal7Client;
using ErrH.Tools.Drupal7Models;
using ErrH.Uploader.Core.DTOs;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.DataAccess
{
    public class RemoteFilesRepo : D7NodesRepoBase<AppFileRepoDto, AppFileNode>
    {

        public RemoteFilesRepo(ID7Client d7Client)
            : base(d7Client)
        { }


        protected override Func<AppFileNode, object>
            GetKey => x => x.Name;



        protected override AppFileNode FromDto(AppFileRepoDto dto)
            => AppFileNode.FromDto(dto, null);

    }
}
