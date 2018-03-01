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
        public EditUser(User user)
        {
            InitializeComponent();
            username.Text = user.Properties["sAMAccountName"].ToString();
            Path.Text = user.Path;
            FullName.Text = user.Name;
            foreach(Group group in user.Groups)
            {
                treeView1.Nodes.Add(group.ToString());
            }
        }
    }
}
