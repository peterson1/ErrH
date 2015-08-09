namespace ErrH.UploaderApp
{
    public enum VsRemote
    {
        /// <summary>
        /// Default state before comparing.
        /// </summary>
        Unknown,

        /// <summary>
        /// While comparing against remote.
        /// </summary>
        Checking,

        /// <summary>
        /// File sizes and hashes are the same.
        /// </summary>
        Same,

        /// <summary>
        /// File size or hash is different.
        /// </summary>
        Changed,

        /// <summary>
        /// Found in remote, but not in local.
        /// </summary>
        NotInLocal,

        /// <summary>
        /// Found in local, but not in remote.
        /// </summary>
        NotInRemote,

        /// <summary>
        /// Multiple app-file nodes in server with the same file name for a given app.
        /// This should never happen in production.
        /// </summary>
        MultipleInRemote
    }
    public enum Sending { Idle, OnGoing, Finished, Error }
}
