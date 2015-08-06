namespace ErrH.Tools.Drupal7Shim
{
    public class Constants
    {
        /// <summary>
        /// Indicates that the file is permanent and should not be deleted.
        /// Temporary files older than DRUPAL_MAXIMUM_TEMP_FILE_AGE will be removed during cron runs, but permanent files will not be removed during the file garbage collection process.
        /// </summary>
        public const int FILE_STATUS_PERMANENT = 1;

        /// <summary>
        /// Temporary files older than DRUPAL_MAXIMUM_TEMP_FILE_AGE will be removed during cron runs, but permanent files will not be removed during the file garbage collection process.
        /// </summary>
        public const int FILE_STATUS_TEMPORARY = 0;
    }
}
