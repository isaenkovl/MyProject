using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Common
{
    public abstract class ADataProvider
    {
        

        
        // Добавление нового контакта 
        public abstract void AddContact(User user, Contact contact);
        // Удаление контакта с базы
        public abstract void DeleteContact(User user, Contact contact);
        //Получить все контакты для текущего пользователя
        public abstract List<Contact> GetContacts(User user);
        
        //удалить все контакты пользователя
        public abstract void ClearContacts(User user);

    }
}
