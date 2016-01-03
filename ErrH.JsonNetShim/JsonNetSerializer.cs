using System;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;

namespace ErrH.JsonNetShim
{
    public class JsonNetSerializer : LogSourceBase, ISerializer
    {
        public T Read<T>(string jsonText, bool raiseLogEvents)
        {
            if (raiseLogEvents)
                Trace_i("Parsing Json as ‹{0}›...", typeof(T).Name);

            T obj; try
            {
                obj = Js_n.Read<T>(jsonText);
            }
            catch (Exception e)
            {
                if (raiseLogEvents)
                    return Error_o_(default(T),
                        "Error in parsing Json string." + L.f + jsonText + L.f + e.Details(true, false));
                else
                    return Error_(default(T),
                        "Error in parsing Json string.", jsonText + L.f + e.Details(true, false));
            }

            if (raiseLogEvents)
                return Trace_o_(obj, "Successfully parsed Json string");
            else
                return obj;
        }


        public T Read<T>(FileShim fileShim, bool raiseLogEvents)
        {
            var s = raiseLogEvents ? fileShim.ReadUTF8 
                                   : fileShim._ReadUTF8;
            if (s.IsBlank())
                return Warn_(default(T), "Invalid Json format.",
                                         "Content of file is blank.");
            return this.Read<T>(s, raiseLogEvents);
        }


        public string SHA1(object obj) => Write(obj, false).SHA1();


        public bool TryRead<T>(string jsonText, out T parsedObj, bool raiseLogEvents)
        {
            parsedObj = Read<T>(jsonText, raiseLogEvents);
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
