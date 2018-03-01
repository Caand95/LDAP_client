using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LDAP
{
    public class User : DirectoryEntity
    {
        private List<Group> groups;
        public List<Group> Groups { get => groups; }

        public User(DirectoryEntity parent, PropertyCollection properties, Group[] groups) : base(parent, properties)
        {
            this.groups = new List<Group>();
            if(groups != null)
                for (int i = 0; i < groups.Length; i++)
                    this.groups.Add(groups[i]);
        }
    }
}
