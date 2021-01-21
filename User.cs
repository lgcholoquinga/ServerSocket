using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    [Serializable]
    public class User
    {
        public string user;
        public string password;
        public User(string _user, string _password)
        {
            this.user = _user;
            this.password = _password;
        }
    }
}
