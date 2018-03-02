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
            string comboText = comboBox1.Text.ToLower();
            foreach (DirectoryEntity entity in directory.entries)
            {
                string prefix = "";
                if (entity.GetType() == typeof(OrganizationalUnit))
                    prefix = "OU";
                else if (entity.GetType() == typeof(User))
                    prefix = "User";
                else if (entity.GetType() == typeof(Group))
                    prefix = "Group";

                bool isProperGroup = false;

                if(comboText.Equals("all"))
                {
                    isProperGroup = true;
                }
                else
                {
                    if(comboText.Substring(0, comboText.Length - 1).Equals(prefix.ToLower()))
                    {
                        isProperGroup = true;
                    }
                }



                if(entity.Name.ToLower().Contains(search) && isProperGroup)
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
                this.listView1.Columns.Add("Property", 100);
                this.listView1.Columns.Add("Value", 300);

                foreach(string property in entity.Properties.PropertyNames)
                {
                    object value = entity.Properties[property].Value;
                    if (value.GetType().IsArray)
                    {
                        Array array = (Array)value;
                        for (int i = 0; i < array.Length; i++)
                        {
                            AddText(property, array.GetValue(i).ToString());
                        }
                    }
                    else
                    {
                        AddText(property, "" + value);

                    }
                }

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
