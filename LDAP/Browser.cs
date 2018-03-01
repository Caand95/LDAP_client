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
            label1.Text = connection.Credentials.Domain;
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.CheckBoxes = true;

            GetView(DirectoryTree.GetRoot(connection), treeView1.Nodes);
        }

        private void GetView(OrganizationalUnit directory, TreeNodeCollection parentNode)
        {
            foreach(DirectoryEntity entity in directory.entries)
            {
                string prefix = "";
                if (entity.GetType() == typeof(OrganizationalUnit))
                    prefix = "OU";
                else if (entity.GetType() == typeof(User))
                    prefix = "User";
                else if (entity.GetType() == typeof(Group))
                    prefix = "Group";

                TreeNode node = parentNode.Add(prefix + " - " + entity.Name);
                node.Tag = entity;
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    GetView((OrganizationalUnit)entity, node.Nodes);
                }

            }
            
        }

        private void GetView(string search, OrganizationalUnit directory, TreeNodeCollection parentNode)
        {
            foreach (DirectoryEntity entity in directory.entries)
            {
                string prefix = "";
                if (entity.GetType() == typeof(OrganizationalUnit))
                    prefix = "OU";
                else if (entity.GetType() == typeof(User))
                    prefix = "User";
                else if (entity.GetType() == typeof(Group))
                    prefix = "Group";

                if(entity.Name.ToLower().Contains(search))
                {
                    TreeNode node = parentNode.Add(prefix + " - " + entity.Name);
                    node.Tag = entity;
                }
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    GetView(search, (OrganizationalUnit)entity, parentNode);
                }

            }

        }

        private List<Control> customControls = new List<Control>();

        private Label CreateLabel(int x, int y, string text)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(x, y);
            label.Name = text;
            label.Size = new System.Drawing.Size(46, 17);
            label.TabIndex = 0;
            label.Text = text;
            this.Controls.Add(label);
            customControls.Add(label);
            return label;
        }

        private void AddText(string name, string value)
        {
            ListViewItem lvi = new ListViewItem(new string[] { name, value});
            this.listView1.Items.Add(lvi);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Action == TreeViewAction.ByMouse)
            {
                for(int i = 0; i < customControls.Count; i++)
                {
                    this.Controls.Remove(customControls[i]);
                }
                customControls.Clear();
                DirectoryEntity entity = ((DirectoryEntity)e.Node.Tag);
                this.listView1.Clear();
                this.listView1.Columns.Add("Property");
                this.listView1.Columns.Add("Value", 300);
                AddText("Name", entity.Name);
                AddText("Path", entity.Path);
                //Label name = CreateLabel(50, 30, entity.Name);
                //Label path = CreateLabel(50, 50, entity.Path);

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            if (textBox1.Text.Replace(' ', '\0').Length < 1)
                GetView(DirectoryTree.GetRoot(connection), treeView1.Nodes);
            else GetView(textBox1.Text, DirectoryTree.GetRoot(connection), treeView1.Nodes);
        }
    }
}
