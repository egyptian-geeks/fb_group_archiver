using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace GroupArchiver
{
    public class Data
    {
        public static string Get(string name)
        {

            XDocument doc = XDocument.Load(Application.StartupPath + "\\data.xml");
            if (doc.Root.Element(name)!=null)
            {
                return doc.Root.Element(name).Value;
            }
            return null;

        }
        public static void Set(string name,string value)
        {

            XDocument doc = XDocument.Load(Application.StartupPath + "\\data.xml");
            if (doc.Root.Element(name) == null)
            {
                 doc.Root.Add(new XElement(name,value));
            }
            else
            {
                doc.Root.Element(name).Value = value;
            }
            doc.Save(Application.StartupPath + "\\data.xml");

        }

        public static void SaveGroup(string id, long lastUpdate, string SavePath)
        {
            XDocument doc = XDocument.Load(Application.StartupPath + "\\data.xml");
            if (doc.Root.Elements("group").Where(c=>c.Element("id").Value==id).Count()!=0)
            {
                XElement foundGroup = doc.Root.Elements("group").Where(c => c.Element("id").Value == id).FirstOrDefault();
                foundGroup.Element("lastupdate").Value = lastUpdate.ToString();
                foundGroup.Element("savepath").Value = SavePath;
            }
            else
            {
                XElement group = new XElement("group");
                group.Add(new XElement("id", id));
                group.Add(new XElement("lastupdate", lastUpdate));
                group.Add(new XElement("savepath", SavePath));
                doc.Root.Add(group);
            }

            doc.Save(Application.StartupPath + "\\data.xml");
        }
    }
}
