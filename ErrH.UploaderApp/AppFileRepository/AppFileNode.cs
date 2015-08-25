
using ErrH.Tools.Extensions;

namespace ErrH.UploaderApp.AppFileRepository
{
    public class AppFileNode
    {
        internal AppNode App    { get; set; }

        public int     Nid      { get; set; }
        public int     Vid      { get; set; }
        public string  Name     { get; set; }

        public string  Version  { get; set; }
        public long    Size     { get; set; }
        public string  SHA1     { get; set; }
                                
        public int     Fid      { get; set; }



        public static AppFileNode FromDto(AppFileRepo_Dto dto, AppNode app)
        {
            if (dto.app_file_nid_vid.IsBlank()) return null;
            return new AppFileNode
            {
                Nid = dto.app_file_nid.GetValueOrDefault(-1),
                Vid = dto.app_file_vid.GetValueOrDefault(-1),
                Name = dto.app_file_name,
                Version = dto.app_file_version,
                Size = dto.app_file_size.GetValueOrDefault(-1),
                SHA1 = dto.app_file_sha1,
                Fid = dto.app_file_fid.GetValueOrDefault(-1),
                App = app
            };
        }
    }
}
