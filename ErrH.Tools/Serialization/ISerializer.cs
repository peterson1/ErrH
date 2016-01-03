using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Serialization
{
    public interface ISerializer : ILogSource
    {
        T Read<T>(string serializedObject, bool raiseLogEvents = true);

        T Read<T>(FileShim fileShim, bool raiseLogEvents = true);

        bool TryRead<T>(string serializedObject, out T parsedObj, bool raiseLogEvents = false);

        string Write(object obj, bool prettyPrint);

        string SHA1(object obj);
    }
}
