using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyPhoneBook.Manager
{
    class Validator
    {
        public static bool EmailCheck(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool PassWordCheck(string password)
        {
            return CheckByRegex(password, Properties.Resources.RegexStringForPassword);
        }

        private static bool CheckByRegex(string text, string pattern)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(text);
            return match.Success;
        }

        public static bool PhoneCheck(string phone)
        {
            return CheckByRegex(phone, Properties.Resources.RegexPhoneNumber);
           
        }

        public static bool ContactNameCheck(string text)
        {
            return text.Length >= 3;
        }
    }
}
