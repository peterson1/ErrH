using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;

namespace ErrH.UploaderApp.AppFileRepository
{

    //later: centralize all compare logic to here
    public static class AppFileItemExtensions
{

	public static bool Matches(this AppFileItem remoteF, FileShim localF, out string difference)
	{
		if (remoteF.Size != localF.Size)
		{
			difference = $"{localF.Size.KB()} (local) -vs- {remoteF.Size.KB()} (remote)";
			return false;
		}
		
		var localSHA1 = localF.SHA1;

		if (remoteF.SHA1 != localSHA1)
		{
			difference = $"{localSHA1} (local) -vs- {remoteF.SHA1} (remote)";
			return false;
		}

		difference = string.Empty;
		return true;
	}


	public static bool Matches(this AppFileItem remoteF, FileShim localF)
	{
		string diff;
		return remoteF.Matches(localF, out diff);
	}

}
}
