using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.Drupal7Models.FieldValues;

namespace ErrH.Tools.Drupal7Models.Fields
{
    public class und
    {

        public static UndTargetId TargetId(int id)
        {
            return new UndTargetId { target_id = id };
        }

        public static FieldUnd<UndTargetId> TargetIds(params int[] ids)
        {
            return new FieldUnd<UndTargetId> { und = ids.Select(x => und.TargetId(x)).ToList() };
        }



        public static UndTermId TermId(int id)
        {
            return new UndTermId { tid = id };
        }

        public static FieldUnd<UndTermId> TermIds(params int[] ids)
        {
            return new FieldUnd<UndTermId> { und = ids.Select(x => und.TermId(x)).ToList() };
        }


        public static UndValue Value(object value)
        {
            return new UndValue { value = value };
        }

        public static FieldUnd<UndValue> Values(params object[] values)
        {
            return new FieldUnd<UndValue> { und = values.Select(x => und.Value(x)).ToList() };
        }


        public static FieldUnd<Und2Values> Value1_2(object fieldValue1, object fieldValue2)
        {
            var ret = new FieldUnd<Und2Values>();

            ret.und.Add(new Und2Values
            {
                value  = fieldValue1,
                value2 = fieldValue2
            });

            return ret;
        }

        //public static FieldUnd<UndFid> fid(params int[] fids) {
        //	return new FieldUnd<UndFid> { und = fids.Select(x => 
        //		new UndFid { fid = x }).ToList() }; }




        public static UndFid Fid(int fid)
        {
            return new UndFid { fid = fid };
        }

        /// <summary>
        /// Use -1 to get an empty element.
        /// </summary>
        /// <param name="fids"></param>
        /// <returns></returns>
        public static FieldUnd<UndFid> Fids(params int[] fids)
        {
            var u = new List<UndFid>();

            foreach (var fid in fids)
                if (fid != -1) u.Add(und.Fid(fid));

            return new FieldUnd<UndFid> { und = u };
        }
    }
}
