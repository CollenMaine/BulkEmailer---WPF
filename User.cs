using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkEmailer___WPF
{
    internal class User
    {
        public string city { get; set; }
        public string cover { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string uid { get; set; }

        public User(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        public string GetDetails()
        {
            if (String.IsNullOrEmpty(name))
                return "NULL_USERNAME" + "\t\t\t:" + email;
            else
                return name + "\t\t\t:" + email;
        }
    }
}
