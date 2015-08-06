using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Serialization
{
    public interface ISerializer : ILogSource
    {
        T Read<T>(string serializedObject);

        T Read<T>(FileShim fileShim);

        bool TryRead<T>(string serializedObject, out T parsedObj);

        string Write(object obj, bool prettyPrint);
    }
}
