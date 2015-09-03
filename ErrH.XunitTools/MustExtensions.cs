using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{
    public static class MustExtensions
    {
        public static ITestOutputHelper OutputHelper { get; set; }


        public static void MustHave(this IEnumerable<int> list, params int[] args)
        {
            foreach (var numbr in args)
                if (!list.Contains(numbr))
                    Assert.True(false, "List does not contain : " + numbr);

            SayPass("List contains all {0} items.", args.Length);
        }


        public static void MustNotBeNull(this object obj, string description)
        {
            if (obj == null) Assert.True(false, description + " is NULL.");
            SayPass(description + " is not null.");
        }


        public static void MustHave(this string fullText, string subString)
        {
            try { Assert.Contains(subString, fullText); }
            catch
            {
                Assert.True(false, $"Expected text to contain: “{subString}”" 
                                 + $"{L.f}Full text: “{fullText}”");
            }
            SayPass($"Text contains “{subString}”. (full text: “{fullText}”)");
        }


        public static void MustBe<T>(this T actual, T expctd, string desc = null)
        {
            Compare1OrMany<T>(expctd, actual, desc);
        }


        //public static List<string> MustBe { get { return null; } }


        private static void Compare1OrMany<T>(T expctd, T actual, string desc = null)
        {
            if (expctd is ICollection)
                CompareLists((ICollection)expctd, (ICollection)actual);
            else
                CompareScalar<T>(expctd, actual, desc);
        }


        private static void CompareLists<T>(T expctdList, T actualList,
                                            bool verifySorting = false
                                            ) where T : ICollection
        {
            if (verifySorting) Throw.Unsupported(verifySorting, "verifySorting");

            var lT = CoalesceType(expctdList, actualList);

            CompareValues(expctdList.Count,
                          actualList.Count,
                          "Item counts of " + lT.ListName().Guillemet());

            var eRows = expctdList.GetEnumerator();
            var aRows = actualList.GetEnumerator();

            for (int i = 0; i < expctdList.Count; i++)
            {
                eRows.MoveNext();
                aRows.MoveNext();

                dynamic e = eRows.Current;
                dynamic a = aRows.Current;
                Type t = CoalesceType(e, a);

                Compare1OrMany(e, a, "{0} [{1}]".f(t.Name, i));
            }
        }


        private static void CompareScalar<T>(T expctd, T actual, string desc = null)
        {
            if (typeof(T).IsNative() || typeof(T).IsEnum)
                CompareValues(expctd, actual, desc);
            else
                CompareProperties<T>(expctd, actual, desc);
        }


        private static void CompareProperties<T>(T expctdObj, T actualObj, string variableDesc = null)
        {
            if (variableDesc.IsBlank())
                variableDesc = typeof(T).Name.Guillemet();

            if (expctdObj == null || actualObj == null)
            {
                CompareValues<T>(expctdObj, actualObj, variableDesc);
                return;
            }

            var props = typeof(T).GetProperties();
            if (props.Length == 0) Throw.NoMember("Comparable properties");

            foreach (var prop in props)
            {
                var e = prop.GetValue(expctdObj);
                var a = prop.GetValue(actualObj);

                //var d = (e == null && a == null)
                //	  ? "{0} . {1} {2}".f(variableDesc, prop.Name, prop.PropertyType.Name.Guillemet())
                //	  : "{0} . {1}".f(variableDesc, prop.Name);

                var d = "{0} . {1}".f(variableDesc, prop.Name);

                CompareValues(e, a, d);
            }
        }



        private static void CompareValues<T>(T expctd, T actual, string description)
        {
            if (description.IsBlank())
                description = "value";//MethodNow.JumpBack(4).Name;

            if (expctd == null && actual == null)
            {
                SayPass("{0}\t ==  Both NULL", description);
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    if (expctd == null && actual.ToString().IsBlank())
                    {
                        SayPass("{0}\t ==  Both BLANK", description);
                        return;
                    }
                }

                try { Assert.Equal(expctd, actual); }
                catch { Fail<T>(description, expctd, actual); }


                var typ = CoalesceType<T>(expctd, actual);
                var val = actual.ToString();

                if (typ == typeof(string))
                    val = (val.IsBlank()) ? "blank".Guillemet() : val.Quotify();

                SayPass("{0}\t ==  {1} {2}", description, val, typ.Name.Guillemet());
            }
        }


        private static void SayPass(string format, params object[] args)
        {
            OutputHelper.WriteLine(" OK : " + format, args);
        }


        private static void Fail<T>(string varDesc, T expctd, T actual)
        {
            var t = CoalesceType<T>(expctd, actual);

            var e = (expctd == null) ? "‹ null ›" : expctd.ToString();
            var a = (actual == null) ? "‹ null ›" : actual.ToString();

            if (t == typeof(string))
            {
                //e = (e.IsBlank()) ? "‹ blank ›" : e.Quotify();
                //a = (a.IsBlank()) ? "‹ blank ›" : a.Quotify();
                if (e.IsBlank()) e = "‹ blank ›";
                if (a.IsBlank()) a = "‹ blank ›";
            }

            var msg = "Mismatch in {0}." + L.f
                    + "Expected :  {2} {1}" + L.f
                    + "Actual :      {3}" + L.f
                    + "-".Repeat(10);

            Assert.True(false, msg.f(varDesc.Guillemets(),
                                    t.Name.Guillemet(), e, a));
        }


        private static Type CoalesceType<T>(T expctd, T actual)
        {
            if (typeof(T) != typeof(object)) return typeof(T);

            if (expctd == null && actual == null)
                Throw.BadArg("Expected and Actual", "should not be both null");

            return ((dynamic)expctd ?? (dynamic)actual).GetType();
            //var sampl = (expctd == null) ? actual : expctd;
            //return sampl.GetType();
        }

    }
}
