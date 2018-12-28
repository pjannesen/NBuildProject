using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Jannesen.VisualStudioExtension.NBuildProject.Build.Library
{
    public sealed class ConfigReader : IDisposable
    {
        private     XmlTextReader                   _xmlReader;

        public      bool                            hasChildren
        {
            get {
                if (_xmlReader.NodeType != XmlNodeType.Element)
                    throw new InvalidOperationException("Invalid node type.");

                return !_xmlReader.IsEmptyElement;
            }
        }
        public      string                          ElementName
        {
            get {
                return _xmlReader.Name;
            }
        }
        public      int                             LineNumber
        {
            get {
                return _xmlReader.LineNumber;
            }
        }

        public                                      ConfigReader(string fileName)
        {
            _xmlReader  = new XmlTextReader(fileName);
        }
        public      void                            Dispose()
        {
            _xmlReader.Dispose();
        }

        public      void                            ReadRootNode(string rootElementName)
        {
            do {
                if (!_xmlReader.Read())
                    throw new ConfigException("Reading EOF while reading XML.");
            }
            while (_xmlReader.NodeType != XmlNodeType.Element);

            if (_xmlReader.Name != rootElementName)
                throw new ConfigException("Invalid root name.");
        }
        public      bool                            ReadNextElement()
        {
            for(;;) {
                if (!_xmlReader.Read())
                    throw new ConfigException("Invalid XML: reading EOF.");

                switch(_xmlReader.NodeType)
                {
                case XmlNodeType.Element:
                    return true;

                case XmlNodeType.EndElement:
                    return false;
                }
            }
        }
        public      void                            NoChildElements()
        {
            if (!_xmlReader.IsEmptyElement) {
                if (ReadNextElement())
                    throw new ConfigException("Invalid XML: element is not empty.");
            }
        }

        public      string[]                        GetAttibutes()
        {
            List<string>        rtn = new List<string>();

            if (_xmlReader.MoveToFirstAttribute()) {
                do {
                    rtn.Add(_xmlReader.Name);
                }
                while (_xmlReader.MoveToNextAttribute());

                _xmlReader.MoveToElement();
            }

            return rtn.ToArray();
        }
        public      string                          GetValueString(string name)
        {
            string      value = _xmlReader.GetAttribute(name);

            if (value == null)
                throw new ConfigException("Missing attribute '" + name + "'.");

            return value;
        }
        public      string                          GetValueString(string name, string defaultValue)
        {
            string      value = _xmlReader.GetAttribute(name);

            if (value == null)
                return defaultValue;

            return value;
        }
        public      bool                            GetValueBool(string name, bool defaultValue)
        {
            string      value = _xmlReader.GetAttribute(name);

            if (value == null)
                return defaultValue;

            switch(value)
            {
            case "0":
            case "n":
                return false;

            case "1":
            case "y":
                return true;

            default:
                throw new ConfigException("Invalid boolean value in attribute '" + name + "'.");
            }
        }
        public      Int64                           GetValueInt64(string name)
        {
            string      value = GetValueString(name);

            try {
                return Int64.Parse(value);
            }
            catch(Exception) {
                throw new ConfigException("Invalid integer value in attribute '" + name + "'.");
            }
        }
        public      DateTime                        GetValueDateTime(string name)
        {
            string      value = GetValueString(name);

            try {
                return DateTime.ParseExact(value, "O", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.RoundtripKind);
            }
            catch(Exception) {
                throw new ConfigException("Invalid integer value in attribute '" + name + "'.");
            }
        }
        public      T                               GetValueEnum<T>(string name) where T: struct
        {
            string      value = _xmlReader.GetAttribute(name);

            if (value == null)
                throw new ConfigException("Missing attribute '" + name + "'.");

            T result;

            try {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch(Exception) {
                    throw new ConfigException("Invalid '" + value + "' value in attribute '" + name + "'.");
            }

            return result;
        }
        public      T                               GetValueEnum<T>(string name, T defaultValue) where T: struct
        {
            string      value = _xmlReader.GetAttribute(name);

            if (value == null)
                return defaultValue;

            T result;

            try {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch(Exception) {
                    throw new ConfigException("Invalid '" + value + "' value in attribute '" + name + "'.");
            }

            return result;
        }
    }
}
