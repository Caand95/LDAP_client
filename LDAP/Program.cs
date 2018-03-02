using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Windows.Forms;

namespace LDAP
{
    class Program
    {
        //Statics();
        static string programname = "LDAP TO TEAMSOCCER";
        static string[] menu =
            {
                    "Close",
                    "Search AD",
                    "Add user",
                    "move user",
                    "delete user"
                };

        static void Main(string[] args)
        {
            Application.Run(new FormConnect());
        }

        static void OldMain(string[] args)
        {
            //INIT();
            Ldap ldap = new Ldap();
            ldap.Init();

            string searchDir;
            string searchFilter;
            string user;
            string pass;
            string userdn;
            string newdn;
            ldap.Connect();

            string menuint;

            //PROGRAM();
            Output(programname);
            bool program = true;
            while (program)
            {
                menuint = Menu(menu);
                switch (menuint)
                {
                    case "0":
                        program = false;
                        break;
                    case "1":
                        searchDir = Input("Please input search Directory: ");
                        searchFilter = Input("Please input filter:");
                        PrintResponse(ldap.SendSearchRequest(searchDir, searchFilter));
                        break;
                    case "2":
                        user = Input("Please Input username: ");
                        pass = Input("Please input password: ");
                        if (ldap.AddUser(user, pass)) {
                            Output("User added");
                        }
                        else { Output("error\nPlease try again"); }
                        break;
                    case "3":
                        user = Input("please input username to move: ");
                        userdn = Input("please input the current user dn: ");
                        newdn = Input("Please intput the new user dn: ");
                        if(ldap.MoveUser(user, userdn, newdn))
                        {
                            Output(user + " moved from " + userdn + " to " + newdn);
                        }
                        else { Output("error\nPlease try again"); }
                        break;
                    case "4":
                        user = Input("please Input username to delete: ");
                        userdn = Input("please input the the full DistinguisedName of " + user + ": ");
                        if (ldap.DeleteUser(user, userdn))
                        {
                            Output("User deleted");
                        } else {Output("error\nPlease try again"); }
                        break;
                    case "-":
                        PrintResponse(ldap.TestRequest());
                        break;
                    default:
                        Output("Something went wrong");
                        break;
                }
                Output("press anykey to return to the menu...");

                Wait();
                Clear();
            }

        }

        //GUI METHODs
        static void Output(string text)
        {
            Console.WriteLine(text);
        }

        static string Input(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        static void Wait()
        {
            Console.ReadKey();
        }

        static void Clear()
        {
            Console.Clear();
        }

        static string Menu(string[] menu)
        {
            Output("\nMenu:");
            for (int i = 1; i < menu.Length; i++)
            {
                Output(i + ". " + menu[i]);
            }
            Output("0. " + menu[0]);
            Output("-. " + "VIEW ALL USERS");
            return Input("\nEnter your choice: ");
        }

        //LOGIC METHODs
        static void PrintResponse(SearchResponse response)
        {
            foreach(SearchResultEntry entry in response.Entries)
            {
                Output(entry.DistinguishedName);
            }
        }
    }
}