/** 
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
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics; 


namespace Dream.Build.Play._2012.Plists
{

    class SpriteSheet
    {
        private static Dictionary<string, PlistDictionary> _sheets =  null;
        private static PlistDictionary _sheet = null;
        Texture2D _texture;
        string _textureCacheKey;


        public SpriteSheet()
        {

        }

        public SpriteSheet(String path, Texture2D spriteSheet)
        {
            Debug.Assert(path != null, "SpriteSheet: Path cannot be null");

            if (_sheet == null)
            { // nothing in spritesheet
                AddInitialSpriteSheet(path);
                _texture = spriteSheet;
            }
        }

        public SpriteSheet(string path, string textureCacheKey)
        {
            Debug.Assert(path != null, "SpriteSheet: Path cannot be null");
            Debug.Assert(textureCacheKey != null, "SpriteSheet: TextureCacheKey cannot be null");

            if (_sheet == null)
            {
                AddInitialSpriteSheet(path);
                textureCacheKey = textureCacheKey;
            }
        }

        void AddInitialSpriteSheet(string path)
        {
            // get file contents as string
            using (StreamReader stream = new StreamReader(path))
            {
                String file = stream.ReadToEnd();

                // ignore dtd processing
                var settings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore,
                };

                // create xml reader from string data
                using (XmlReader reader = XmlReader.Create(new StringReader(file), settings))
                {
                    // Assert file is plist
                    Debug.Assert(reader.ReadToDescendant("plist"));
                    // Process plist
                    ProcessPlist(reader);   
                }
            }
        }

        private void ProcessPlist(XmlReader reader)
        {   
            // create null plist object
            PlistObjectBase rootDict = null;
            
            // loop through reader
            while (reader.Read())
            {
                // node type is not an Element ignore and loop again.
                if (reader.NodeType != XmlNodeType.Element) continue;
                // node type is element, loop through xml data recursively.
                rootDict = LoadFromNode(reader); 
             }
             
            // assign loaded plist as sheet.
             _sheet = (PlistDictionary)rootDict;
            

        }
        /*
         *  Recursive method to load xml plist data into plist objects.
         * 
         */
        private PlistObjectBase LoadFromNode(XmlReader reader)
        {
            //Console.WriteLine("Loading data from node");
            //only processes XmlNodeType.Element
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
                    return new PlistArray();
                case "key":
                    return new PlistString(reader.ReadElementContentAsString());
                case "string":
                    return new PlistString(reader.ReadElementContentAsString());
                case "integer":
                    return new PlistInteger(reader.ReadElementContentAsInt());
                case "real":
                    return new PlistInteger(reader.ReadElementContentAsInt());
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

        /* Helper method to read through a xml dict element.
         * 
         * 
         */

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

        /* determine if sheet contains sprite frames.
         * 
         * 
         */
        public bool HasFrames()
        {   
            // determine whether plist dictionary contains frames.
            return _sheet.ContainsKey("frames");
        }

        /*
         *  return a Frame struct by frame name.
         * 
         * 
         * 
         */

        public Frame SpriteFrameByFrameName(String name)
        {
            // get frames PlistDictionary and get PlistDictionary for frame name.
            PlistDictionary frames = (PlistDictionary)_sheet.TryGetValue("frames");
            PlistDictionary coords = (PlistDictionary)frames.TryGetValue(name);

            // create frame struct and populate with data plist dictionary.
            Frame frame;
            frame.x = ((PlistInteger)coords.TryGetValue("x")).Value;
            frame.y = ((PlistInteger)coords.TryGetValue("y")).Value;
            frame.offsetX = ((PlistInteger)coords.TryGetValue("offsetX")).Value;
            frame.offsetY = ((PlistInteger)coords.TryGetValue("offsetY")).Value;
            frame.originalWidth = ((PlistInteger)coords.TryGetValue("originalWidth")).Value;
            frame.originalHeight = ((PlistInteger)coords.TryGetValue("originalHeight")).Value;
            frame.width = ((PlistInteger)coords.TryGetValue("width")).Value;
            frame.height = ((PlistInteger)coords.TryGetValue("height")).Value;

            return frame;
        
        }

        /* For debugging information
         * 
         */
        public void WriteContentsToConsole()
        {
            Console.WriteLine("writing contents...sheet has " + _sheet.Count + " items.");
            PlistDictionary frames = (PlistDictionary)_sheet.TryGetValue("frames");
            PlistDictionary image = (PlistDictionary)frames.TryGetValue("bee2.png");
            foreach (KeyValuePair<string, PlistObjectBase> item in image)
            {
                Console.WriteLine("key " + item.Key);
            }
            Console.WriteLine("");
        }

        /* ***********************************
         * Methods Relating to Animation
         * 
         *************************************/


        /*
         * Create animation object from sprite sheet and frame names.
         * 
         * 
         */

        public Animation AnimationForFrameNames(String[] frameNames, int frameTime, Color frameColor, bool isLooping)
        {
            // create animation object.
            Animation animation = new Animation();
            // create frames array.
            Frame[] frames = new Frame[frameNames.Length];

            // loop through frameNames and poplulate Frame array.
            for ( int i = 0; i < frameNames.Length; i++ )
            {
                frames[i] = SpriteFrameByFrameName(frameNames[i]);   
            }

            // initialize animation and return.
            animation.Initialize(_texture, frames, frameTime, frameColor, isLooping);
            return animation;
        }


    }
}
