using System;
using ErrH.BinUpdater.Core.DTOs;
using ErrH.Drupal7Client;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;

namespace ErrH.BinUpdater.DataAccess
{
    public class RemoteFilesRepo : D7NodesRepoBase<AppFileRepoDto, SyncableFileRemote>
    {

        public RemoteFilesRepo()
        {
            Added += (s, e) =>
            {
                var i = e.Value;
                Trace_n("Added item:", $"‹{i.GetType().Name}› “{i.Name}”");
            };
        }


        protected override Func<SyncableFileRemote, object>
            GetKey => x => x.Name;



        protected override SyncableFileRemote FromDto(AppFileRepoDto dto)
        {
            var ret = new SyncableFileRemote
            {
                Name    = dto.app_file_name,
                Version = dto.app_file_version,
                Size    = dto.app_file_size.GetValueOrDefault(-1),
                SHA1    = dto.app_file_sha1,
                Fid     = dto.app_file_fid.GetValueOrDefault(-1),
            };

            if (!ParseNidVid(dto, ret)) return null;

            return ret;
        }


        private bool ParseNidVid(AppFileRepoDto dto, SyncableFileRemote ret)
        {
            var s = dto.app_file_nid_vid;
            if (!s.Contains(".")) goto invalid;

            var ss = s.Split('.');
            if (ss.Length != 2 || !ss[0].IsNumeric()
                               || !ss[1].IsNumeric()) goto invalid;

            ret.Nid = ss[0].ToInt();
            ret.Vid = ss[1].ToInt();
            return true;

            invalid:  return Error_n("Unexpected nid_vid format:", s);
        }



        //public override bool Add(AppFileNode node)
        //{
        //    var dto = AppFileDto.From(node);
        //    //_client.Post(dto);
        //    return true;
        //}


    }
}
