using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Diagnostics;

namespace LDAP
{
    class Ldap
    {
        private NetworkCredential credentials;
        private LdapDirectoryIdentifier serverId;
        private LdapConnection connection;
        //TESTCOMMENT
        private SearchResponse response;
        private SearchRequest request;
        private AddRequest addrequest;
        private DeleteRequest delrequest;

        public void Init()
        {
            credentials = new NetworkCredential(@"Administrator", "Team Soccer1", "Team-Soccer.local");
            serverId = new LdapDirectoryIdentifier("192.168.0.1", 389);
        }

        public bool Connect()
        {
            try
            {
                connection = new LdapConnection(serverId, credentials);
                connection.Bind();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddUser(string username, string passwd)
        {
            try
            {
                addrequest = new AddRequest("cn=" + username + ",CN=Users,dc=Team-Soccer,dc=Local", new DirectoryAttribute[] {
            new DirectoryAttribute("cn", username),
            new DirectoryAttribute("ou", "users"),
            new DirectoryAttribute("userPassword", passwd),
            new DirectoryAttribute("objectClass", new string[] {"top","person", "organizationalPerson", "user" })
            });
                this.connection.SendRequest(addrequest);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool DeleteUser(string username, string userdn)
        {
            if (username != "Administrator") {
                try
                {
                    delrequest = new DeleteRequest(userdn);
                    this.connection.SendRequest(delrequest);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
        }
            return true;
        }
        public SearchResponse SendSearchRequest(string path, string filter)
        {
            try
            {
                Debug.WriteLine("Making request");
                request = new SearchRequest(path+",dc=team-soccer,dc=local", "(objectClass="+filter+")", System.DirectoryServices.Protocols.SearchScope.Subtree, null);
                Debug.WriteLine("Getting Response");
                response = (SearchResponse)connection.SendRequest(request);
            }
            catch (Exception)
            {
                Debug.WriteLine("error at testresponse");
            }
            return response;
        }

        public SearchResponse TestRequest()
        {
            try
            {
                Debug.WriteLine("Making request");
                request = new SearchRequest("cn=users,dc=team-soccer,dc=local", "(objectClass=user)", System.DirectoryServices.Protocols.SearchScope.Subtree, null);
                Debug.WriteLine("Getting Response");
                response = (SearchResponse)connection.SendRequest(request);
            }
            catch (Exception)
            {
                Debug.WriteLine("error at testresponse");
            }
            return response;
        }

        public void Clean()
        {
            connection.Dispose();
        }

    }
}
