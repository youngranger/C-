using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace YrUti
{
    public static class Serialization
    {
        public static XmlDocument JsonList2Xml(List<String> jsonList)
        {
            XmlDocument xmldoc = new XmlDocument();//创建xml对象
            XmlNode declarationNode = xmldoc.CreateXmlDeclaration("1.0", "utf-8", "");//添加声明
            xmldoc.AppendChild(declarationNode);
            XmlElement tables = xmldoc.CreateElement("entityInfo");//根节点用工程类别
            xmldoc.AppendChild(tables);//创建根节点
            foreach (var json in jsonList)
            {
                XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                foreach (var node in doc.DocumentElement.ChildNodes)
                {
                    xmldoc.DocumentElement.AppendChild(xmldoc.ImportNode((XmlNode)node, true));
                }
            }

            return xmldoc;
        }

        public static XmlDocument Json2Xml(String json)
        {
            XmlDocument xmldoc = new XmlDocument();//创建xml对象
            XmlNode declarationNode = xmldoc.CreateXmlDeclaration("1.0", "utf-8", "");//添加声明
            xmldoc.AppendChild(declarationNode);
            xmldoc.CreateElement("json");
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            foreach (var node in doc.DocumentElement.ChildNodes)
            {
                xmldoc.DocumentElement.AppendChild(xmldoc.ImportNode((XmlNode)node, true));
            }

            return xmldoc;
        }
    }
}
