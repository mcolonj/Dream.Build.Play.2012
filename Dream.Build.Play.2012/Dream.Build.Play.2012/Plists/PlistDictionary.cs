using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsGame1.Plists
{
    class PlistDictionary : PlistObjectBase, IEnumerable<KeyValuePair<string, PlistObjectBase>>
    {

        private List<string> keys;
        private Dictionary<string, PlistObjectBase> dict = new Dictionary<string, PlistObjectBase>();

        public PlistDictionary()
            : this(false)
        {

        }

        public PlistDictionary(bool keepOrder)
        {
            if (keepOrder)
            {
                keys = new List<string>();
            }
        }

        public PlistDictionary(Dictionary<string, PlistObjectBase> value)
            : this(value, false)
        {
        }

        public PlistDictionary(Dictionary<string, PlistObjectBase> value, bool keepOrder)
            : this(keepOrder)
        {
            foreach (KeyValuePair<string, PlistObjectBase> item in value)
            {
                Add(item.Key, item.Value);
            }
        }

        public PlistDictionary(IDictionary value)
            : this(value, false)
        {
        }

        public PlistDictionary(IDictionary value, bool keepOrder)
            : this(keepOrder)
        {
            foreach (DictionaryEntry item in value)
            {
                Add((string)item.Key, ObjectToPlistObject(item.Value));
            }
        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("dict");
            foreach (KeyValuePair<string, PlistObjectBase> item in this)
            {
                writer.WriteElementString("key", item.Key);
                item.Value.Write(writer);
            }
            writer.WriteEndElement();
        }

        public void Clear()
        {
            if (keys != null)
            {
                keys.Clear();
            }

            dict.Clear();
        }

        public void Add(string key, PlistObjectBase value)
        {
            if (keys != null)
            {
                keys.Add(key);
            }

            if (dict.ContainsKey(key))
            {
                Console.WriteLine("Warning:ingoring duplicate key:{0} ( null? {1} empty? {2})", key, key == null, key == "");
            }
            else
            {
                dict.Add(key, value);
            }

        }

        public bool Remove(string key)
        {
            if (keys != null)
            {
                keys.Remove(key);
            }

            return dict.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public PlistObjectBase this[string key]
        {
            get { return dict[key]; }
            set
            {
                if (keys != null)
                {
                    if (!dict.ContainsKey(key))
                    {
                        keys.Add(key);
                    }
                }
                dict[key] = value;
            }

        }

        public PlistObjectBase TryGetValue(string key)
        {
            PlistObjectBase value;
            if (dict.TryGetValue(key, out value))
                return value;

            return null;
        }

        public int Count
        {
            get { return dict.Count; }
        }

        private IEnumerator<KeyValuePair<string, PlistObjectBase>> GetEnumeratorFromKeys()
        {
            foreach (string key in keys)
            {
                yield return new KeyValuePair<string, PlistObjectBase>(key, dict[key]);
            }
        }

        public IEnumerator<KeyValuePair<string, PlistObjectBase>> GetEnumerator()
        {
            return keys == null ? dict.GetEnumerator() : GetEnumeratorFromKeys();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
