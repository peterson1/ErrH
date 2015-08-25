using System.Threading.Tasks;

namespace ErrH.Tools.Extensions
{
    public static class ObjectExtensions
    {
        public static Task<T> ToTask<T>(this T obj)
            => new Task<T>(() => { return obj; });
    }
}
