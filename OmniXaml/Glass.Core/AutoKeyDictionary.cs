namespace OmniXaml.Glass.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AutoKeyDictionary<TKey, TValue> : IEnumerable<TValue>
    {
        readonly IDictionary<TKey, TValue> dict;
        private readonly Func<TKey, TKey> getNextKey;
        private readonly Func<TKey, bool> condition;

        public AutoKeyDictionary(Func<TKey, TKey> getNextKey, Func<TKey, bool> condition) : this(getNextKey, condition, new Dictionary<TKey, TValue>())
        {
        }

        public AutoKeyDictionary(Func<TKey, TKey> getNextKey, Func<TKey, bool> condition, IDictionary<TKey, TValue> dictionary)
        {
            this.getNextKey = getNextKey;
            dict = dictionary;
            this.condition = condition;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                var success = TryGetValue(key, out value);

                if (success)
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }

            set
            {
                dict.Add(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dict.TryGetValue(key, out value) || TryResolveFromHierarchy(key, out value);
        }

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
        }

        private bool TryResolveFromHierarchy(TKey key, out TValue value)
        {
            bool success = false;
            value = default(TValue);

            while (condition(key) && !success)
            {
                success = dict.TryGetValue(key, out value);
                key = getNextKey(key);
            }

            return success;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}