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
    public partial class ConfirmDeleteUser : Form
    {
        Connection connection;
        User user;

        public ConfirmDeleteUser(User user, Connection connection)
        {
            this.connection = connection;
            this.user = user;

            InitializeComponent();
            Confirm.Text = "Are you sure you\nwant to delete\n" + user.Name;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.DeleteUser(user.Properties["distinguishedName"].Value.ToString());
                this.Hide();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
