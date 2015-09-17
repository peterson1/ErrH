namespace ErrH.Uploader.Core
{
    public enum FileDiff
    {
        /// <summary>
        /// Default state before comparing.
        /// </summary>
        Unavailable,

        /// <summary>
        /// Found in remote, but not in local.
        /// </summary>
        NotInLocal,

        /// <summary>
        /// Found in local, but not in remote.
        /// </summary>
        NotInRemote,

        /// <summary>
        /// A criteria was found different.
        /// </summary>
        Changed,

        /// <summary>
        /// All diff criteria match.
        /// </summary>
        Same,
    }


    public enum Action
    {
        Ignore,
        Analyze,

        Create,
        Replace,
        Delete,
    }

    public enum Target
    {
        Unknown,
        Remote,
        Local,
        Both
    }
}
