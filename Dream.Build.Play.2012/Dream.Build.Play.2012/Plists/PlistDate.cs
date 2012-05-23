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
    class PlistDate : PlistObject<DateTime>
    {
        public PlistDate(DateTime value)
            : base(value)
        {

        }

        static readonly string plistDateFormat = "yyyy-mm-ddThh:mm:ssZ";

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("date", Value.ToUniversalTime().ToString(plistDateFormat));
        }

    }
}
