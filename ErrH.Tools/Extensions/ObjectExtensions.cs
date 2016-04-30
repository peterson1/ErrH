using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.SqlHelpers;

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


        public static int ToInt(this object value)
        {
            Throw.IfNull(value, "value to be converted ToInt()");
            return value.ToString().ToInt();
        }


        /// <summary>
        /// Converts value to nullable int?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ToInt_(this object value)
            => value.ToString().ToInt_();



        public static decimal ToDec(this object value)
        {
            Throw.IfNull(value, "value to be converted ToDecimal()");
            return value.ToString().ToDec();
        }


        public static void CopyValuesFrom(this object dest, ResultRow source)
        {
            if ((dest == null) || (source == null)) return;


            var writableProps = dest.GetType().WritableProps();
            foreach (var key in source.Keys)
            {
                var prop = writableProps.SingleOrDefault(x 
                    => x.Name.ToLower() == key.ToLower());
                if (prop != null)
                {
                    try
                    {
                        prop.SetValue(dest, source[key], null);
                    }
                    catch
                    {
                    }
                }
            }
        }


        public static void CopyValuesFrom(this object dest, object source)
        {
            if ((dest == null) || (source == null)) return;

            var srcTyp = source.GetType();
            foreach (var destProp in dest.GetType().WritableProps())
            {
                var srcProp = srcTyp.GetProperty(destProp.Name, destProp.PropertyType);
                if (srcProp == null) continue;
                var srcVal = srcProp.GetValue(source, null);
                destProp.SetValue(dest, srcVal, null);
            }
        }
    }
}
