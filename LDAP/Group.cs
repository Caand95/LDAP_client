using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LDAP
{
    public class Group : DirectoryEntity
    {
        private List<User> users;
        public Group(DirectoryEntity parent, PropertyCollection properties) : base(parent, properties)
        {
            this.users = new List<User>();
        }
    }
}
