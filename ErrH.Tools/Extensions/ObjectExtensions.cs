using System.Threading.Tasks;

namespace ErrH.Tools.Extensions
{
    public static class ObjectExtensions
    {
        public static Task<T> ToTask<T>
            (this T obj, bool runSynchronously = true)
        {
            var t = new Task<T>(() => { return obj; });
            if (runSynchronously) t.RunSynchronously();
            return t;
        }
    }
}
