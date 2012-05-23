using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Plists
{
    class PlistData : PlistObject<byte[]>
    {
        public PlistData(string value)
            : base(Convert.FromBase64String(value))
        {

        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("data");
            writer.WriteBase64(Value, Value.Length, 1);
            writer.WriteEndElement();
        }

    }
}
