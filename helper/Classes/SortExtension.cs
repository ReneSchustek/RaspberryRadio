using System.Collections.Generic;

namespace Helper.Classes
{
    /// <summary>
    /// Sortiert ein Dictionary nach den Key
    /// </summary>
    public static class SortExtension
    {
        public static IDictionary<TKey, TValue> SortBy<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys
        )
        {
            var sorter = new KeyComparer<TKey>(keys);
            return new SortedDictionary<TKey, TValue>(dictionary, sorter);
        }

        public static IDictionary<TKey, TValue> SortByDesc<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys
        )
        {
            var sorter = new DescendingKeyComparer<TKey>(keys);
            return new SortedDictionary<TKey, TValue>(dictionary, sorter);
        }
    }

    public class DescendingKeyComparer<T> : KeyComparer<T>
    {
        public DescendingKeyComparer(IEnumerable<T> keys) : base(keys)
        {
        }

        public override int Compare(T x, T y)
        {
            return base.Compare(x, y) * -1;
        }
    }

    public class KeyComparer<T> : IComparer<T>
    {
        private const int TOP = -1;
        private const int BOTTOM = 1;
        private const int EQUAL = 0;

        private readonly Dictionary<T, int> _keys = new Dictionary<T, int>();

        public KeyComparer(IEnumerable<T> keys)
        {
            if (keys != null)
                foreach (var key in keys)
                    _keys.Add(key, _keys.Count);
        }

        public virtual int Compare(T x, T y)
        {
            int iX = -1, iY = -1;

            if (_keys.TryGetValue(x, out var aX))
                iX = aX;

            if (_keys.TryGetValue(y, out var aY))
                iY = aY;

            if (iX == -1)
                return BOTTOM;

            if (iX == iY)
                return EQUAL;

            if (iY == -1 || iX < iY)
                return TOP;

            return BOTTOM;
        }
    }
}
