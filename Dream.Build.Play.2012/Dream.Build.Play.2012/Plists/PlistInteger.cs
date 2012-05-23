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
    class PlistInteger : PlistObject<int>
    {
        public PlistInteger(int value)
            : base(value)
        {

        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("integer ", Value.ToString());
        }
    }
}
