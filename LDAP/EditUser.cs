using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            username.Text = user.Properties["sAMAccountName"].Value.ToString();
            password.Text = "********";
            Path.Text = user.Path;
            FirstName.Text = user.Properties["givenName"].Value.ToString();
            LastName.Text = user.Properties["sn"].Value.ToString();

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
            try
            {
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), username.Text, "sAMAccountName");
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), FirstName.Text, "givenName");
                connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), LastName.Text, "sn");
                //connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), FirstName.Text + " " + username.Text + ". " + LastName.Text, "displayName");
                //connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), FirstName.Text + " " + username.Text + ". " + LastName.Text, "cn");
                //connection.ModifyEntryValue(user.Properties["distinguishedName"].Value.ToString(), FirstName.Text + " " + username.Text + ". " + LastName.Text, "name");
            }
            catch (Exception error)
            {
                MessageBox.Show("Error editing user\n\n" + error.Message);
            }
            MessageBox.Show("Done editing user");
        }
    }
}
