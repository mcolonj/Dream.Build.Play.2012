/** 
*
* Taken from : Aaron Bockover <abockover@novell.com> https://github.com/mono/monodevelop/tree/master/main/src/addins/MonoDevelop.MacDev/MonoDevelop.MacDev.Plist,
* Who took from: PodSleuth (http://git.gnome.org/cgit/podsleuth)
* -------------------------------------------------------------
* HighBeta Dream.Build.Play 2012 http://github.com/mcolonj/Dream.Build.Play.2012
*
* Copyright (c) Michael Colon
* Copyright (c) 2012 HighBeta, LLC.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
**/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Dream.Build.Play._2012.Plists
{
    class PlistDocument : PlistObjectBase
    {
        private const string version = "1.0";

        private PlistObjectBase root;

        public PlistDocument()
        {
        }

        public PlistDocument(PlistObjectBase root)
        {
            this.root = root;
        }

        public void LoadFromXmlFile(string path)
        {
            //Console.WriteLine(path);
            //allow DTD but not try to resolve it from web
            var settings = new XmlReaderSettings()
            {

                //XmlResolver = null,
                DtdProcessing = DtdProcessing.Parse
            };
            using (var reader = XmlReader.Create(path, settings))
            {

                LoadFromXml(reader);
            }
        }

        public void LoadFromXml(string data)
        {
            //allow DTD but not try to resolve it from web
            var settings = new XmlReaderSettings()
            {
                CloseInput = true,
                XmlResolver = null,
                DtdProcessing = DtdProcessing.Ignore 
            };

            using (var reader = XmlReader.Create(new StringReader(data), settings))
            {
                LoadFromXml(reader);
            }
        }

        public void LoadFromXml(XmlReader reader)
        {
            
            reader.ReadToDescendant("plist");
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element) continue;
                //Console.WriteLine(reader.LocalName);
                //Console.WriteLine(reader.ToString());
                if (!reader.EOF)
                    root = LoadFromNode(reader);
            }
        }

        private PlistObjectBase LoadFromNode(XmlReader reader)
        {
            //Debug.Assert(reader.NodeType == XmlNodeType.Element);
            bool isEmpty = reader.IsEmptyElement;
            switch (reader.LocalName)
            {
                case "dict":
                    var dict = new PlistDictionary(true);
                    if (!isEmpty)
                    {
                        if (reader.ReadToDescendant("key"))
                            dict = LoadDictionaryContents(reader, dict);
                        reader.ReadEndElement();
                    }
                    return dict;

                case "array":
                    if (isEmpty)
                        return new PlistArray();

                    //advance to first node
                    reader.ReadStartElement();
                    while (reader.Read() && reader.NodeType != XmlNodeType.Element) ;

                    // HACK: plist data in iPods is not even valid in some cases! Way to go Apple!
                    // This hack checks to see if they really meant for this array to be a dict.
                    if (reader.LocalName == "key")
                    {
                        var ret = LoadDictionaryContents(reader, new PlistDictionary(true));
                        reader.ReadEndElement();
                        return ret;
                    }

                    var arr = new PlistArray();
                    do
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var val = LoadFromNode(reader);
                            if (val != null)
                                arr.Add(val);
                        }
                    } while (reader.Read() && reader.NodeType != XmlNodeType.EndElement);
                    reader.ReadEndElement();
                    return arr;

                case "key":
                    return new PlistString(reader.ReadElementContentAsString());
                case "string":
                    return new PlistString(reader.ReadElementContentAsString());
                case "integer":
                    return new PlistInteger(reader.ReadElementContentAsInt());
                case "real":
                    return new PlistReal(reader.ReadElementContentAsDouble());
                case "false":
                    reader.ReadStartElement();
                    if (!isEmpty)
                        reader.ReadEndElement();
                    return new PlistBoolean(false);
                case "true":
                    reader.ReadStartElement();
                    if (!isEmpty)
                        reader.ReadEndElement();
                    return new PlistBoolean(true);
                case "data":
                    return new PlistData(reader.ReadElementContentAsString());
                case "date":
                    return new PlistDate(reader.ReadElementContentAsDateTime());
                default:
                    throw new XmlException(String.Format("Plist Node `{0}' is not supported", reader.LocalName));
            }
        }

        private PlistDictionary LoadDictionaryContents(XmlReader reader, PlistDictionary dict)
        {
            Debug.Assert(reader.NodeType == XmlNodeType.Element && reader.LocalName == "key");
            while (!reader.EOF && reader.NodeType == XmlNodeType.Element)
            {
                string key = reader.ReadElementString();
                while (reader.NodeType != XmlNodeType.Element && reader.Read())
                    if (reader.NodeType == XmlNodeType.EndElement)
                        throw new Exception(String.Format("No value found for key {0}", key));
                PlistObjectBase result = LoadFromNode(reader);
                if (result != null)
                    dict.Add(key, result);
                reader.ReadToNextSibling("key");
            }
            return dict;
        }

        public PlistObjectBase Root
        {
            get { return root; }
            set { root = value; }
        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteDocType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", version);
            root.Write(writer);
            writer.WriteEndDocument();
        }

        public void WriteToFile(string filename)
        {
            using (var writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                Write(writer);
            }
        }
    }
}
