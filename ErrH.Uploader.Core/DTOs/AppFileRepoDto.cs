namespace ErrH.Uploader.Core.DTOs
{
    public class AppFileRepoDto
    {
        public string  app_nid_vid       { get; set; }
        public int     app_nid           { get; set; }
        public int     app_vid           { get; set; }
        public string  app_title         { get; set; }
        public string  app_users         { get; set; }

        public string  app_file_nid_vid  { get; set; }
        public int?    app_file_nid      { get; set; }
        public int?    app_file_vid      { get; set; }
        public string  app_file_name     { get; set; }
        public string  app_file_version  { get; set; }
        public long?   app_file_size     { get; set; }
        public string  app_file_sha1     { get; set; }
        public int?    app_file_fid      { get; set; }
    }
}
