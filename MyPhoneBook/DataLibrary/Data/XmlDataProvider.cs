using DataLibrary.Common;
using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataLibrary.Data
{
     public class XmlDataProvider : ADataProvider
    {
        //путь файла xml для хранения данных
        private string path = Properties.Settings.Default.XmlFilePath;

        public override void AddContact(User currentUser, Contact contact)
        {
            XmlDocument doc = InitDocument(path);
            XmlElement root = doc.DocumentElement;

            XmlNode newContact = doc.CreateElement("contact");
            XmlAttribute date = doc.CreateAttribute("date");
            date.Value = DateTime.Now.ToString();
            XmlAttribute phoneNumber = doc.CreateAttribute("number");
            phoneNumber.Value = contact.PhoneNumber;
            XmlAttribute contactName = doc.CreateAttribute("name");
            contactName.Value = contact.ContactName;
            XmlAttribute description = doc.CreateAttribute("description");
            description.Value = contact.Description;
            newContact.Attributes.Append(date);
            newContact.Attributes.Append(phoneNumber);
            newContact.Attributes.Append(contactName);
            newContact.Attributes.Append(description);

            bool complete = false;


            foreach (XmlElement user in root.ChildNodes)
            {
                if (user.Name == "user")
                {
                    string email = user.GetAttributeNode("email").Value;
                    if (currentUser.EmailAddress == email)
                    {
                        user.AppendChild(newContact);
                        complete = true;
                    }
                  
                }
            }
            //если это первый контакт пользователя
            if (!complete)
            {
                XmlNode newUser = doc.CreateElement("user");
                XmlAttribute email = doc.CreateAttribute("email");
                email.Value = currentUser.EmailAddress;
                newUser.Attributes.Append(email);
                newUser.AppendChild(newContact);
                root.AppendChild(newUser);
            }

             doc.Save(path);
        }

        public override void DeleteContact(User currentUser, Contact contact)
        {
            XmlDocument doc = InitDocument(path);
            XmlElement root = doc.DocumentElement;

            foreach (XmlElement user in root.ChildNodes)
            {
                if (user.Name == "user")
                {
                    string email = user.GetAttributeNode("email").Value;
                    if (currentUser.EmailAddress == email)
                    {
                        foreach (XmlElement contactNode in user.ChildNodes)
                        {
                            if(contactNode.GetAttributeNode("number").Value == contact.PhoneNumber)
                            {
                                user.RemoveChild(contactNode);
                            }
                        }
                       
                    }

                }
            }
            
            doc.Save(path);
        }

        public override void ClearContacts(User currentUser)
        {
            XmlDocument doc = InitDocument(path);
            XmlElement root = doc.DocumentElement;

            foreach (XmlElement user in root.ChildNodes)
            {
                if (user.Name == "user")
                {
                    string email = user.GetAttributeNode("email").Value;
                    if (currentUser.EmailAddress == email)
                    {
                        user.RemoveAll();

                        XmlAttribute newEmail = doc.CreateAttribute("email");
                        newEmail.Value = currentUser.EmailAddress;
                        user.Attributes.Append(newEmail);
                    }

                }
            }

            doc.Save(path);
        }

        public override List<Contact> GetContacts(User currentUser)
        {
            List<Contact> result = new List<Contact>();

            XmlDocument doc = InitDocument(path);
            XmlElement root = doc.DocumentElement;

            foreach (XmlElement user in root.ChildNodes)
            {
                if (user.Name == "user")
                {
                    string email = user.GetAttributeNode("email").Value;
                    if (currentUser.EmailAddress == email)
                    {
                        foreach (XmlElement contactNode in user.ChildNodes)
                        {
                            Contact contact = new Contact();
                            contact.ContactName = contactNode.GetAttributeNode("name").Value;
                            contact.PhoneNumber = contactNode.GetAttributeNode("number").Value;
                            contact.Description = contactNode.GetAttributeNode("description").Value;
                            contact.DataCreate =  DateTime.Parse(contactNode.GetAttributeNode("date").Value);

                            result.Add(contact);

                        }

                    }

                }
            }

            return result;
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
        private XmlDocument CreateXmlFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("data");
            
            doc.AppendChild(root);
            doc.Save(path);
            return doc;
        }

        
    }
}
