using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
     public class Contact
    {
        public string BindingDataTime
        {
            get
            {
                return DataCreate.ToString("dd.MM.yyyy HH:mm");
            }
            set { }
        }
        public DateTime DataCreate { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string Description { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Contact other = obj as Contact;
            if (other == null) return false;
            return other.PhoneNumber.Equals(this.PhoneNumber) && other.ContactName.Equals(this.ContactName);

        }
        public override int GetHashCode()
        {
            return PhoneNumber.GetHashCode() + ContactName.GetHashCode();
        }
        public override string ToString()
        {
            return String.Format("{0} | {1} | {2}", ContactName, PhoneNumber, Description);
        }

    }
}
