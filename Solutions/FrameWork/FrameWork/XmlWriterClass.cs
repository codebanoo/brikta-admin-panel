using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;

namespace FrameWork
{
    public class XmlWriterClass
    {

        private string uri;

        private string startElement;

        public string Uri
        {
            get
            {
                return this.uri;
            }
            set
            {
                this.uri = value;
            }
        }

        public XmlWriterClass(string uri, string startElement)
        {
            this.uri = uri;
            this.startElement = startElement;
            if (!File.Exists(this.uri))
            {
                using (XmlTextWriter writer = new XmlTextWriter(this.uri, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement(this.startElement);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        public Exception WriteToXml(string parent, XElement element)
        {
            try
            {
                var doc = XDocument.Load(this.uri);
                doc.Element(parent).Add(element);
                doc.Save(this.uri);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        public Exception UpdateXml(XElement doc, XElement element, string itemName, string newValue)
        {
            try
            {
                element.Element(itemName).Value = newValue.ToString();
                doc.Save(this.uri);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        public Exception UpdateXml(XElement doc, XElement element, string itemName1, string newValue1, string itemName2, string newValue2)
        {
            try
            {
                element.Element(itemName1).Value = newValue1.ToString();
                element.Element(itemName2).Value = newValue2.ToString();
                doc.Save(this.uri);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        public Exception UpdateXml(XElement doc, XElement element, string itemName1, string newValue1, string itemName2, string newValue2, string itemName3, string newValue3)
        {
            try
            {
                element.Element(itemName1).Value = newValue1.ToString();
                element.Element(itemName2).Value = newValue2.ToString();
                element.Element(itemName3).Value = newValue3.ToString();
                doc.Save(this.uri);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        public Exception UpdateXml(XElement doc, XElement element, string itemName1, string newValue1, string itemName2, string newValue2, string itemName3, string newValue3, string itemName4, string newValue4)
        {
            try
            {
                element.Element(itemName1).Value = newValue1.ToString();
                element.Element(itemName2).Value = newValue2.ToString();
                element.Element(itemName3).Value = newValue3.ToString();
                element.Element(itemName4).Value = newValue4.ToString();
                doc.Save(this.uri);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }
    }
}