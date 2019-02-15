using DataLibrary.Common;
using DataLibrary.Data;
using DataLibrary.Entities;
using DataLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyPhoneBook.Manager
{
    public class EntitiesManager
    {
       
        private User currentUser;
        public User CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                Contacts = new ObservableCollection<Contact>(dataProvider.GetContacts(CurrentUser));
            }
        }
        private ADataProvider dataProvider;
        private AUserDataProvider userDataProvider;
        

        public ObservableCollection<Contact> Contacts { get; set; }



        public EntitiesManager(DataProvider provider)
        {
            switch (provider)
            {
                case DataProvider.XML:
                    dataProvider = new XmlDataProvider();
                    userDataProvider = new XmlUserDataProvider();
                    break;
                   
            }

        }

        public bool HasPhoneNumber(string phone)
        {
            foreach (Contact contact in Contacts)
            {
                if (contact.PhoneNumber == phone)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasContactName(string name)
        {
            foreach (Contact contact in Contacts)
            {
                if (contact.ContactName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool RegistrationUser(string email, string password)
        {
            bool result = false;
            
            userDataProvider.AddUser(email, GetHash(password));
            result = true;
          
            return result;
        }

        public bool LoginUser(string email, string password)
        {
            bool result = false;
           
            string hash = GetHash(password);
            result = userDataProvider.LoginUser(email, hash);
           
            return result;
        }

        private static string GetHash(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return hash;
        }

        public bool ExistUser(string email)
        {
           return  userDataProvider.ExistsUser(email);
        }

        public void AddContact(User user, Contact contact)
        {
            dataProvider.AddContact(user, contact);
            Contacts.Add(contact);
        }

        public void ClearContacts(User user)
        {
            dataProvider.ClearContacts(user);
            Contacts.Clear();
        }
        public void DeleteContact(Contact contact)
        {
            dataProvider.DeleteContact(CurrentUser, contact);
            Contacts.Remove(contact);
        }
    }

}
