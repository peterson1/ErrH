using System;
using ErrH.Tools.Extensions;
using Newtonsoft.Json;

namespace ErrH.JsonNetShim
{
    public class Js_n
    {
        public static T Read<T>(string jsonText)
        {
            try {
                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            catch (JsonSerializationException ex)
            {
                var m = $"Unable to deserialize JSON to type ‹{typeof(T).ListName()}›."
                      + $"{L.F}   “{ex.Message}”  {L.f}  -->  {ex.InnerException?.Message ?? "‹null›"}"
                      + $"{L.F}    JSON text:     {L.f}       {jsonText}   {L.f}";

                throw new InvalidCastException(m, ex);
            }
        }


        public static string Write(object obj, bool indented = true)
        {
            return JsonConvert.SerializeObject(obj,
                        indented ? Formatting.Indented
                                 : Formatting.None);
        }
    }
}
