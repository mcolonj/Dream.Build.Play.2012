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
    abstract class PlistObjectBase
    {
        public abstract void Write(System.Xml.XmlWriter writer);

        protected static PlistObjectBase ObjectToPlistObject(object value)
        {
            if (value is string)
            {
                return new PlistString((string)value);
            }
            else if (value is bool)
            {
                return new PlistBoolean((bool)value);
            }
            else if (value is double)
            {
                return new PlistReal((double)value);
            }
            else if (value is int)
            {
                return new PlistInteger((int)value);
            }
            else if (value is IEnumerable)
            {
                return new PlistArray((IEnumerable)value);
            }
            else if (value is IDictionary)
            {
                return new PlistDictionary((IDictionary)value);
            }

            throw new InvalidCastException(String.Format("`{0}' cannot be converted to a PlistObjectBase", value.GetType()));
        }

        public static implicit operator PlistObjectBase(string value)
        {
            return new PlistString(value);
        }

        public static implicit operator PlistObjectBase(int value)
        {
            return new PlistInteger(value);
        }

        public static implicit operator PlistObjectBase(double value)
        {
            return new PlistReal(value);
        }

        public static implicit operator PlistObjectBase(bool value)
        {
            return new PlistBoolean(value);
        }

        public static implicit operator PlistObjectBase(object[] value)
        {
            return new PlistArray(value);
        }

        public static implicit operator PlistObjectBase(ArrayList value)
        {
            return new PlistArray(value);
        }

        public static implicit operator PlistObjectBase(Hashtable value)
        {
            return new PlistDictionary(value);
        }

    }
}
