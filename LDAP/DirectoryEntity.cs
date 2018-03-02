using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAP
{
    public abstract class DirectoryEntity
    {
        //The parent OrganizationalUnit
        private DirectoryEntity parent;
        //Properties that defines this object
        private PropertyCollection properties;

        public DirectoryEntity Parent { get => parent; }
        public string Name { get => properties["name"][0].ToString(); }
        public string Path { get => properties["distinguishedName"][0].ToString(); }
        public PropertyCollection Properties { get => properties; }

        public DirectoryEntity(DirectoryEntity parent, PropertyCollection properties)
        {
            this.parent = parent;
            this.properties = properties;
        }
    }
}
