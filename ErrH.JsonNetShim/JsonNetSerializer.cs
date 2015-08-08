using System;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;

namespace ErrH.JsonNetShim
{
    public class JsonNetSerializer : LogSourceBase, ISerializer
    {
        public T Read<T>(string jsonText)
        {
            Trace_i("Parsing Json as ‹{0}›...", typeof(T).Name);
            T obj; try
            {
                obj = Js_n.Read<T>(jsonText);
            }
            catch (Exception e)
            {
                return Error_o_(default(T),
                    "Error in parsing Json string." + L.f + jsonText + L.f + e.Message(true, false));
            }
            return Trace_o_(obj, "Successfully parsed Json string");
        }


        public T Read<T>(FileShim fileShim)
        {
            var s = fileShim.ReadUTF8;
            if (s.IsBlank())
                return Warn_(default(T), "Invalid Json format.",
                                         "Content of file is blank.");
            return this.Read<T>(s);
        }


        public bool TryRead<T>(string jsonText, out T parsedObj)
        {
            parsedObj = Read<T>(jsonText);
            return !parsedObj.Equals(default(T));
        }


        public string Write(object obj, bool indented)
        {
            //Dbg("Serializing object to JSON...", "type: " + obj.GetType().Name.Guillemet());

            var content = Js_n.Write(obj, indented);
            //Trace(content);

            //Debug("Object successfully serialized to JSON.", "size: " + content.Length.KB());
            return content;
        }


    }
}
