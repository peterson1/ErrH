namespace ErrH.Tools.Drupal7Models.Entities
{
    public class D7File
    {

        /// <summary>
        /// File ID
        /// </summary>
        public int fid { get; set; }


        /// <summary>
        /// The {users}.uid of the user who is associated with the file.
        /// </summary>
        public int uid { get; set; }


        /// <summary>
        /// URI of the file.
        /// e.g.: "private://app-files/cmd.exe"
        /// </summary>
        public string uri { get; set; }


        /// <summary>
        /// The size of the file in bytes.
        /// </summary>
        public long filesize { get; set; }


        /// <summary>
        /// A bitmapped field indicating the status of the file.
        /// The first 8 bits are reserved for Drupal core.
        /// The least significant bit indicates temporary (0) or permanent (1). 
        /// Temporary files older than DRUPAL_MAXIMUM_TEMP_FILE_AGE will be removed during cron runs.
        /// </summary>
        public int status { get; set; }


        /// <summary>
        /// UNIX timestamp for the date the file was added to the database.
        /// </summary>
        public long timestamp { get; set; }
        public string uri_full { get; set; } // direct download
        public string target_uri { get; set; }



        /// <summary>
        /// Name of the file with no path components. 
        /// This may differ from the basename of the filepath if the file is renamed to avoid overwriting an existing file.
        /// </summary>
        public string filename { get; set; }


        /// <summary>
        /// The file's MIME type.
        /// </summary>
        public string filemime { get; set; }




        /// <summary>
        /// Base64-encoded string
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// e.g:  "public://my_image.jpg"
        /// </summary>
        public string filepath { get; set; }
    }
}
