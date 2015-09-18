using System.Threading.Tasks;
using ErrH.Tools.ErrorConstructors;

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


        public static T As<T>(this object obj)
            where T : class
        {
            T casted = obj as T;

            if (casted == null)
                throw Error.BadCast<T>(obj);

            return casted;
        }

    }
}
