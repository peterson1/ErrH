using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Randomizers;

namespace ErrH.Tools.Extensions
{
    public static class CollectionExtensions
    {

        public static T RandomItem<T>(this IEnumerable<T> list)
        {
            //var i = new FakeFactory().Integer(0, list.Count() - 1);
            var i = ThreadSafe.LocalRandom.Next(list.Count() - 1);
            return list.ElementAt(i);
        }


        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> list, int itemCount)
        {
            return list.Shuffle().Take(itemCount);
        }


        // from http://stackoverflow.com/a/1262619/3973863
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> ordered)
        {
            var shuffled = ordered.ToList();
            var n = shuffled.Count();

            while (n > 1)
            {
                n--;
                int k = ThreadSafe.LocalRandom.Next(n + 1);
                T value = shuffled[k];
                shuffled[k] = shuffled[n];
                shuffled[n] = value;
            }

            return shuffled;
        }


        public static bool IsAllUnique<T>(this IEnumerable<T> list)
        {
            return list.Distinct().Count() == list.Count();
        }


        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }


        public static bool Has<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            if (list == null) return false;
            if (list.Count() == 0) return false;
            return list.Count(predicate) > 0;
        }


        public static ChildList<TItem, TRoot> ToChildList<TItem, TRoot>(this IEnumerable<TItem> list)
        {
            return new ChildList<TItem, TRoot>(list.ToList());
        }


        public static ReadOnlyCollection<TChild> AsReadOnly<TChild, TParent>
            (this IEnumerable<TChild> enumerable, TParent root)
        {
            var lockdL = enumerable.ToChildList<TChild, TParent>();
            if (lockdL == null) return null;

            lockdL.Parent = root;

            return lockdL as ReadOnlyCollection<TChild>;
        }

    }
}
