using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LDAP
{
    public partial class EditUser : Form
    {

        Connection connection;
        User user;

        public EditUser(User user, Connection connection)
        {
            this.connection = connection;
            this.user = user;

            InitializeComponent();

            //Set the values of the fields
            HeadLine.Text = "Edit " + user.Name;
            username.Text = user.Properties["sAMAccountName"].Value.ToString();
            password.Text = "********";
            Path.Text = user.Path;
            FirstName.Text = user.Properties["givenName"].Value.ToString();
            LastName.Text = user.Properties["sn"].Value.ToString();

            //Check if user has member of and if so add them to the treeview
            if (user.Properties["memberOf"].Count > 0)
            {
                foreach (string memberOf in user.Properties["memberOf"])
                {
                    treeView1.Nodes.Add(memberOf);
                } 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Edit user using the values in the fields
            try
            {
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), username.Text, "sAMAccountName");
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), FirstName.Text, "givenName");
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), LastName.Text, "sn");
            }
            catch (Exception error)
            {
                MessageBox.Show("Error editing user\n\n" + error.Message);
            }
            MessageBox.Show("Done editing user");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    Application.Run(new ConfirmDeleteUser(user, connection));
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error cannot open that type \n" + error.Message);
                }
            }).Start();
        }
    }
}
