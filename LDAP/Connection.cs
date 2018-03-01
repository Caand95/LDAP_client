﻿using System;
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
    public class Connection
    {
        private NetworkCredential credentials;
        private LdapDirectoryIdentifier serverId;
        private LdapConnection ldapConnection;

        public NetworkCredential Credentials { get => credentials; }
        public LdapDirectoryIdentifier Server { get => serverId; }

        public Connection(string domainOrIP, string username, string password)
        {
            credentials = new NetworkCredential(username, password, domainOrIP);
            serverId = new LdapDirectoryIdentifier(domainOrIP, 389);
            ldapConnection = new LdapConnection(serverId, credentials);
            ldapConnection.Bind();
        }

        public DirectoryResponse SendRequest(DirectoryRequest request)
        {
            return ldapConnection.SendRequest(request);
        }

        public void Stop()
        {
            ldapConnection.Dispose();
        }

        public void Reestablish()
        {
            ldapConnection.Bind();
        }

        public string GetDistinguishedName()
        {
            string result = "";
            string[] dotSplitted = credentials.Domain.Split('.');
            for(int i = 0; i < dotSplitted.Length; i++)
            {
                result += "DC=" + dotSplitted[i] + (i < dotSplitted.Length - 1 ? "," : "");
            }
            return result;
        }
    }
}
