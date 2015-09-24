using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.FileSystemShims;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.Core.DTOs
{
    public class AppFileDtoRevision : AppFileDto, ID7NodeRevision
{
    public int vid { get; set; }


    public static AppFileDtoRevision From(AppFileNode node, FileShim file)
    {
        return new AppFileDtoRevision {
            nid                         = node.Nid,
            vid                         = node.Vid,
            title                       = file.Name,
            field_private_file          = und.Fids(node.Fid),
            field_semantic_ver          = und.Values(file.Version),
            field_uploaded_bytes_length = und.Values(file.Size),
            field_sha1_hash             = und.Values(file.SHA1)
        };
    }


    public static new AppFileDtoRevision From(AppFileNode orig)
    {
        return new AppFileDtoRevision {
            nid    =  orig.Nid,
            vid    =  orig.Vid,
            title  =  orig.Name,

            field_private_file          = und.Fids(orig.Fid),
            field_semantic_ver          = und.Values(orig.Version),
            field_uploaded_bytes_length = und.Values(orig.Size),
            field_sha1_hash             = und.Values(orig.SHA1)
        };
    }
}
}
