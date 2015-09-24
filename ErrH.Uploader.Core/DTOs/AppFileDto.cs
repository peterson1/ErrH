using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.Core.DTOs
{
    public class AppFileDto : D7NodeBase
{

    public FieldUnd<UndFid>    field_private_file           { get; set; }
    public FieldUnd<UndValue>  field_semantic_ver           { get; set; }
    public FieldUnd<UndValue>  field_uploaded_bytes_length  { get; set; }
    public FieldUnd<UndValue>  field_sha1_hash              { get; set; }

    public AppFileDto() { this.type = "app_file"; }


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


        public static AppFileDto From(AppFileNode src)
            => new AppFileDto {
                title                       = src.Name,
                field_private_file          = und.Fids(src.Fid),
                field_semantic_ver          = und.Values(src.Version),
                field_uploaded_bytes_length = und.Values(src.Size),
                field_sha1_hash             = und.Values(src.SHA1)
            };
    }
}
