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
    class PlistReal : PlistObject<double>
    {

        public PlistReal(double value)
            : base(value)
        {

        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("real", Value.ToString());
        }

    }
}
