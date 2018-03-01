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
        public static OrganizationalUnit GetRoot(Connection connection)
        {
            Console.WriteLine("Domain Name: " + connection.Credentials.Domain);
            Console.WriteLine("Username: " + connection.Credentials.UserName);
            Console.WriteLine("Password: " + connection.Credentials.Password);
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + connection.Credentials.Domain, connection.Credentials.UserName, connection.Credentials.Password);
            OrganizationalUnit ou = Version2(null, entry);
            PrintOU(ou);
            
            return ou;
        }

        private static void PrintOU(OrganizationalUnit ou)
        {
            Console.WriteLine("OU");
            for (int i = 0; i < ou.entries.Count; i++)
            {
                DirectoryEntity entity = ou.entries[i];
                Console.WriteLine(entity.GetType() + " - " + entity.Name);
                if (entity.GetType() == typeof(OrganizationalUnit))
                    PrintOU((OrganizationalUnit)entity);
            }
        }
        private static OrganizationalUnit Version2(OrganizationalUnit parentEntity, DirectoryEntry parent)
        {
            OrganizationalUnit ou = new OrganizationalUnit(parentEntity, parent.Properties["name"][0].ToString(), parent.Path);
            foreach (DirectoryEntry entry in parent.Children)
            {
                string typeName = entry.SchemaEntry.Name;

                if (typeName.StartsWith("group"))
                {
                    ou.entries.Add(new Group(ou, entry.Name, entry.Path));

                }
                else if (typeName.StartsWith("user"))
                {
                    ou.entries.Add(new User(ou, entry.Name, entry.Path, (string)entry.Properties["sAMAccountName"][0], null));

                }
                else if (typeName.StartsWith("organization"))
                {
                    ou.entries.Add(Version2(new OrganizationalUnit(ou, entry.Name, entry.Path), entry));
                }
            }
            return ou;
        }
    }
}
