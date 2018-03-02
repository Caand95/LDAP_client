using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

namespace LDAP
{
    public class DirectoryTree
    {
        //Gets the top OU from the Domain
        public static OrganizationalUnit GetRoot(Connection connection)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + connection.Credentials.Domain, connection.Credentials.UserName, connection.Credentials.Password);
            OrganizationalUnit ou = GetDirectory(null, entry);
            return ou;
        }

        /*Iterates over the whole Active Directory starting from the parent and loops recursively.
            It initializes an OU and adds users/groups/OU*/
        private static OrganizationalUnit GetDirectory(OrganizationalUnit parentEntity, DirectoryEntry parent)
        {
            OrganizationalUnit ou = new OrganizationalUnit(parentEntity, parent.Properties);
            foreach (DirectoryEntry entry in parent.Children)
            {
                string name = parent.Properties["name"][0].ToString();
                string typeName = entry.SchemaEntry.Name;

                if (typeName.StartsWith("group"))
                {
                    ou.entries.Add(new Group(ou, entry.Properties));

                }
                else if (typeName.StartsWith("user"))
                {
                    ou.entries.Add(new User(ou, entry.Properties));
                }
                else if (typeName.StartsWith("organization"))
                {
                    ou.entries.Add(GetDirectory(new OrganizationalUnit(ou, entry.Properties), entry));
                }
            }
            return ou;
        }
    }
}
