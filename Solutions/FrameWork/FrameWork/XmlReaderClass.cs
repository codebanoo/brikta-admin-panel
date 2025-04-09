using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace FrameWork
{
    public static class XmlReaderClass
    {
        public static IEnumerable<XElement> StreamElements(string uri, string name)
        {
            using (XmlReader reader = XmlReader.Create(uri))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) &&
                      (reader.Name == name))
                    {
                        XElement element = (XElement)XElement.ReadFrom(reader);
                        yield return element;
                    }
                }
                reader.Close();
            }
        }
    }
}