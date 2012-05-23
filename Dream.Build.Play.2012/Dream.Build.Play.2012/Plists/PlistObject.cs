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
    abstract class PlistObject<T> : PlistObjectBase
    {
        private T value;

        public PlistObject(T value)
        {
            Value = value;
        }

        public virtual T Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
