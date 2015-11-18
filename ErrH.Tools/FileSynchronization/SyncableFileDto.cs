using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Drupal7Models.FieldValues;

namespace ErrH.Tools.FileSynchronization
{
    public class SyncableFileDto : D7NodeBase
    {

        public FieldUnd<UndFid>    field_private_file           { get; set; }
        public FieldUnd<UndValue>  field_semantic_ver           { get; set; }
        public FieldUnd<UndValue>  field_uploaded_bytes_length  { get; set; }
        public FieldUnd<UndValue>  field_sha1_hash              { get; set; }



        public SyncableFileDto()
        {
            this.type = "app_file";
        }


        public SyncableFileDto(SyncableFileLocal inf, int fileID) 
            : this()
        {
            title                       = inf.Name;
            field_private_file          = und.Fids(fileID);
            field_semantic_ver          = und.Values(inf.Version);
            field_uploaded_bytes_length = und.Values(inf.Size);
            field_sha1_hash             = und.Values(inf.SHA1);
        }






        //public static AppFileDto From(FileShim file, int fid)
        //{
        //	return new AppFileDto {
        //		title						= file.Name,
        //		field_private_file          = und.Fids(fid),
        //		field_semantic_ver          = und.Values(file.Version),
        //		field_uploaded_bytes_length = und.Values(file.Size),
        //		field_sha1_hash             = und.Values(file.SHA1)
        //	};
        //}


        //public static SyncableFileDto From(AppFileNode src)
        //    => new SyncableFileDto {
        //        title                       = src.Name,
        //        field_private_file          = und.Fids(src.Fid),
        //        field_semantic_ver          = und.Values(src.Version),
        //        field_uploaded_bytes_length = und.Values(src.Size),
        //        field_sha1_hash             = und.Values(src.SHA1)
        //    };
    }
}
