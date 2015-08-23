using ErrH.Tools.ErrorConstructors;

namespace ErrH.Tools.Helpers
{
    public class Cast
    {
        public static T As<T>(object obj)
            where T : class
        {
            T casted = obj as T;

            if (casted == null)
                throw Error.BadCast<T>(obj);

            return casted;
        }
    }
}
