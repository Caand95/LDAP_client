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
    public partial class Browser : Form
    {
        private Connection connection;

        public Browser(Connection connection)
        {
            this.connection = connection;
            InitializeComponent();

            GetView();
        }

        private void GetView()
        {
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
