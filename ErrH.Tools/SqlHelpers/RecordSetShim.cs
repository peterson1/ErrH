using System.Collections.Generic;
using ErrH.Tools.Extensions;
using ErrH.Tools.Serialization;

namespace ErrH.Tools.SqlHelpers
{
    public class RecordSetShim : List<ResultRow>
    {

        //public string SHA1(ISerializer serializer)
        //{
        //    if (Count == 0) return null;
        //    var s = serializer.Write((IEnumerable<ResultRow>)this, false);
        //    return s.SHA1();
        //}
    }
}
