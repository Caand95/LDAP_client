using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAP
{
    public class User : DirectoryEntity
    {
        private List<Group> groups;
        public List<Group> Groups { get => groups; }

        private string username;
        public string Username { get => username; }

        /*
        private string password;
        public string Password { get => password; }
        */

        public User(DirectoryEntity parent, string name, string path, string username, Group[] groups) : base(parent, name, path)
        {
            this.username = username;
            this.groups = new List<Group>();
            if(groups != null)
                for (int i = 0; i < groups.Length; i++)
                    this.groups.Add(groups[i]);
        }

        /** Sets the path making the given OU parent to this object */
        public User(DirectoryEntity parent, string name, OrganizationalUnit ou, string username, Group[] groups) :
            this(parent, name, "CN=" + username + ',' + ou.Path, username, groups)
        { }
    }
}
