using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Common
{
    public abstract class AUserDataProvider
    {
        // Внесение пользователя в базу
        public abstract void AddUser(string userEmail, string password);
        // Метод проверки совпадения почты нового пользователя с существующими
        public abstract bool ExistsUser(string userEmail);
        // В случае успешной авторизации возврат - true
        public abstract bool LoginUser(string userEmail, string password);
    }
}
