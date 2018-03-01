using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

namespace LDAP
{
    public class OrganizationalUnit : DirectoryEntity
    {
        public List<DirectoryEntity> entries;

        public OrganizationalUnit(DirectoryEntity parent, PropertyCollection properties) : base(parent, properties)
        {
            this.entries = new List<DirectoryEntity>();
        }

        public void AddUser(Connection connection, User user)
        {
            
        }

    }
}
