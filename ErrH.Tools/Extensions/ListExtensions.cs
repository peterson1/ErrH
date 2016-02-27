using System.Collections.Generic;

namespace ErrH.Tools.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, IEnumerable<T> newItems)
        {
            list.Clear();
            foreach (var item in newItems)
            {
                list.Add(item);
            }
        }
    }
}
