using ErrH.Tools.Extensions;

namespace ErrH.Tools.ReflectionHelpers
{
    public class EmbeddedResource
    {
        public static string Get<T> (string resourcePath
                                   , string lineBreakReplacement = null
                                   , string tabReplacement = null
                                   )
            => typeof(T).ReadEmbedded(resourcePath, 
                lineBreakReplacement, tabReplacement);
    }
}
