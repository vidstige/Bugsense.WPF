using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bugsense.WPF
{
    [Serializable]
    internal class JsonDictionary<TKey, TValue> : ISerializable
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public JsonDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }
        public JsonDictionary(SerializationInfo info, StreamingContext context)
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }
        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }
        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (TKey key in _dictionary.Keys)
                info.AddValue(key.ToString(), _dictionary[key]);
        }
    }
}
