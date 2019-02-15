using DataLibrary.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataLibrary.Data
{
    public class XmlUserDataProvider : AUserDataProvider
    {



        public override void AddUser(string userEmail, string password)
        {
            XmlDocument doc = InitDocument(Properties.Settings.Default.XmlUserFilePath);
            XmlElement root = doc.DocumentElement;


            XmlNode userNode = doc.CreateElement("user");
            XmlAttribute emailAttribute =  doc.CreateAttribute("email");
            XmlAttribute passAttribute = doc.CreateAttribute("password");
            emailAttribute.Value = userEmail;
            passAttribute.Value = password;

            userNode.Attributes.Append(emailAttribute);
            userNode.Attributes.Append(passAttribute);

            root.AppendChild(userNode);
            doc.Save(Properties.Settings.Default.XmlUserFilePath);

        }

        // Метод проверки на двойника
        public override bool ExistsUser(string userEmail)
        {
            XmlElement root = InitDocument(Properties.Settings.Default.XmlUserFilePath).DocumentElement;
            foreach (XmlElement user in root.ChildNodes)
            {
                if (user.Name == "user")
                {
                   string email =  user.GetAttributeNode("email").Value;
                    if (email == userEmail)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        //Проверка корректности введенных данных
        public override bool LoginUser(string userEmail, string password)
        {
            XmlElement root = InitDocument(Properties.Settings.Default.XmlUserFilePath).DocumentElement;
            foreach (XmlElement user in root.ChildNodes)
            {
                if(user.Name == "user")
                {
                    string email = string.Empty;
                    string passwordUser = string.Empty;
                    foreach(XmlAttribute attributes in user.Attributes)
                    {
                        if (attributes.Name == "email")
                        {
                            email = attributes.Value;
                        }

                        if (attributes.Name == "password")
                        {
                            passwordUser = attributes.Value;
                        }
                    }
                    if(userEmail == email && password == passwordUser)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private XmlDocument InitDocument(string filePath)
        {
            XmlElement root;
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.Load(filePath);
                root = doc.DocumentElement;
            }
            catch (FileNotFoundException ex)
            {
                doc = CreateXmlFile(filePath);
                doc.Load(filePath);
                root = doc.DocumentElement;
            }



            return doc;
        }
        // Создаем пустую Базу
        private XmlDocument CreateXmlFile(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration  xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("users");
            XmlNode userNode = doc.CreateElement("user");

            XmlAttribute email = doc.CreateAttribute("email");
            XmlAttribute password = doc.CreateAttribute("password");

            userNode.Attributes.Append(email);
            userNode.Attributes.Append(password);

            root.AppendChild(userNode);
            doc.AppendChild(root);
            doc.Save(filepath);
            return doc;
        }
    }
}
