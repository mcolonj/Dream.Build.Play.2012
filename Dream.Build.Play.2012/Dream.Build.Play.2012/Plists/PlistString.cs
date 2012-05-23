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
    class PlistString : PlistObject<string>
    {
        public PlistString(string value)
            : base(value)
        {

        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("string", Value.ToString());
        }
    }
}
