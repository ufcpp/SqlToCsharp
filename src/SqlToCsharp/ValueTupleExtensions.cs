namespace SqlToCsharp
{
    using System.Collections.Generic;

    static class TupleExtensions
    {
        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> items)
        {
            int index = 0;
            foreach (var item in items)
            {
                yield return (item, index);
                ++index;
            }
        }
    }
}
