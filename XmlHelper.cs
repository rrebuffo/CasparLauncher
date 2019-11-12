using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CasparLauncher
{
    public class XmlHelper
    {

        public static void NewDocument(out XmlDocument document, out XmlNode rootNode, string baseNode = null)
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xml.DocumentElement;
            xml.InsertBefore(xmlDeclaration, root);
            if (baseNode != null)
            {
                XmlNode node = xml.CreateElement(string.Empty, baseNode, string.Empty);
                xml.AppendChild(node);
                rootNode = node;
            }
            else
            {
                rootNode = null;
            }
            document = xml;
        }
        
        public static XmlNode NewNode(XmlDocument xml, string nodename, XmlNode append = null)
        {
            XmlNode newnode = xml.CreateElement(string.Empty, nodename, string.Empty);
            if (append != null) append.AppendChild(newnode);
            return newnode;
        }

        public static XmlAttribute NewAttribute(XmlDocument xml, string attrname, string value, XmlNode append = null)
        {
            XmlAttribute newattr = xml.CreateAttribute(string.Empty, attrname, string.Empty);
            newattr.Value = value;
            if (append != null) append.Attributes.Append(newattr);
            return newattr;
        }

        public static XmlNode NewTextNode(XmlDocument xml, string nodename, string value, XmlNode append = null)
        {
            XmlNode newnode = xml.CreateElement(string.Empty, nodename, string.Empty);
            XmlText newtextnode = xml.CreateTextNode(value);
            newnode.AppendChild(newtextnode);
            if(append != null) append.AppendChild(newnode);
            return newnode;
        }

    }
}
