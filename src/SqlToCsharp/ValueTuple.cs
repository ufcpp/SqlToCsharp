namespace System
{
    public struct ValueTuple
    {
        public static ValueTuple<T1> Create<T1>(T1 item1) => new ValueTuple<T1>(item1);
        public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new ValueTuple<T1, T2>(item1, item2);
    }

    public struct ValueTuple<T1>
    {
        public T1 Item1;

        public ValueTuple(T1 item1)
        {
            Item1 = item1;
        }
    }

    public struct ValueTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Event)]
    public sealed class TupleElementNamesAttribute : Attribute
    {
        private readonly string[] _transformNames;
        public TupleElementNamesAttribute(string[] transformNames)
        {
            _transformNames = transformNames ?? throw new ArgumentNullException(nameof(transformNames));
        }
        public IList<string> TransformNames => _transformNames;
    }
}

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
