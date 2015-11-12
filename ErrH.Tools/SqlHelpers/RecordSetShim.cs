using System.Collections.Generic;
using ErrH.Tools.Extensions;
using ErrH.Tools.Serialization;

namespace ErrH.Tools.SqlHelpers
{
    public class RecordSetShim : List<ResultRow>
    {
        private ISerializer _serializr;



        public RecordSetShim(ISerializer serializer)
        {
            _serializr = serializer;
        }


        public string SHA1()
        {
            if (Count == 0) return null;
            var s = _serializr.Write((IEnumerable<ResultRow>)this, false);
            return s.SHA1();
        }
    }
}
