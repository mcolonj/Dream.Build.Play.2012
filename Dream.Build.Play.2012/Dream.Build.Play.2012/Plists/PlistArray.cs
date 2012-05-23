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
    class PlistArray : PlistObject<List<PlistObjectBase>>, IEnumerable<PlistObjectBase>
    {

        public PlistArray()
            : base(new List<PlistObjectBase>())
        {

        }

        public PlistArray(List<PlistObjectBase> value)
            : base(value)
        {

        }

        public PlistArray(IEnumerable value)
            : this()
        {
            foreach (object item in value)
            {
                Add(ObjectToPlistObject(item));
            }
        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("array");
            foreach (PlistObjectBase o in this)
            {
                o.Write(writer);
            }
            writer.WriteEndElement();
        }

        public void Clear()
        {
            Value.Clear();
        }

        public void Add(PlistObjectBase value)
        {
            Value.Add(value);
        }

        public void Add(IDictionary value)
        {
            Value.Add(new PlistDictionary(value));
        }

        public bool Remove(PlistObjectBase value)
        {
            return Value.Remove(value);
        }

        public bool Contains(PlistObjectBase value)
        {
            return Value.Contains(value);
        }

        public int Count
        {
            get { return Value.Count; }
        }

        public IEnumerator<PlistObjectBase> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

    }
}
