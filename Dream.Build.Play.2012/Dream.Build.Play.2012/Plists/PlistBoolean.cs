using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Plists
{
    class PlistBoolean : PlistObject<bool>
    {
        public PlistBoolean(bool value)
            : base(value)
        {

        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("string", Value.ToString());
        }

    }
}
