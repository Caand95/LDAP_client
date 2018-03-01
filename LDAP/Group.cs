using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAP
{
    public class Group : DirectoryEntity
    {
        private List<User> users;
        public Group(DirectoryEntity parent, string name, string path) : base(parent, name, path)
        {
            this.users = new List<User>();
        }
    }
}
