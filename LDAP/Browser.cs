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

            GetView(DirectoryTree.GetRoot(connection), null, treeView1.Nodes);
        }

        private void GetView(OrganizationalUnit directory, TreeNode parentNode, TreeNodeCollection nodeCollection)
        {
            foreach(DirectoryEntity entity in directory.entries)
            {
                if (parentNode == null)
                {
                    if (entity.GetType() == typeof(OrganizationalUnit))
                    {
                        TreeNode newParentNode = nodeCollection.Add(entity.Name);
                        GetView((OrganizationalUnit)entity, newParentNode, nodeCollection);
                    }
                    else nodeCollection.Add(entity.Name);
                }
                else
                {
                    string prefix = "";
                    if (entity.GetType() == typeof(OrganizationalUnit))
                        parentNode.Nodes.Add(entity.Name);
                }
            }
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
