using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class User
    {
        public string EmailAddress { get; set; }

        public override string ToString()
        {
            return EmailAddress;
        }
    }
}
