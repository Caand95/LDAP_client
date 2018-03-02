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
    public partial class Browser : Form
    {
        private Connection connection;

        public Browser(Connection connection)
        {
            this.connection = connection;
            InitializeComponent();
            label1.Text = connection.Credentials.Domain;

            //Makes the listview a column view
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

<<<<<<< HEAD
            GetView(DirectoryTree.GetRoot(connection), treeView1.Nodes);
            treeView1.NodeMouseDoubleClick += EditUser;
=======
            //Adds initial content to the tree view from Active Directory
            GetView(Directory.GetRoot(connection), treeView1.Nodes);
>>>>>>> b8d6a0b716e60c2e987b5dd689b6db8265774a39
        }

        //Fills the tree-view with the directory items recursively
        private void GetView(OrganizationalUnit directory, TreeNodeCollection parentNode)
        {
            foreach(DirectoryEntity entity in directory.entries)
            {
                string prefix = "";
                int imageIndex = 0; //Icon
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    prefix = "OU";
                    imageIndex = 0;
                }
                else if (entity.GetType() == typeof(Group))
                {
                    prefix = "Group";
                    imageIndex = 1;
                }
                else if (entity.GetType() == typeof(User))
                {
                    prefix = "User";
                    imageIndex = 2;
                }

                TreeNode node = parentNode.Add(prefix + " - " + entity.Name);
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
                node.Tag = entity;
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    GetView((OrganizationalUnit)entity, node.Nodes);
                }
            }
        }

        //Fills the tree-view with the directory items recursively and allows search
        private void GetView(string search, OrganizationalUnit directory, TreeNodeCollection parentNode)
        {
            string comboText = comboBox1.Text.ToLower();
            foreach (DirectoryEntity entity in directory.entries)
            {
                string prefix = "";
                int imageIndex = 0; //Icon
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    prefix = "OU";
                    imageIndex = 0;
                }
                else if (entity.GetType() == typeof(Group))
                {
                    prefix = "Group";
                    imageIndex = 1;
                }
                else if (entity.GetType() == typeof(User))
                {
                    prefix = "User";
                    imageIndex = 2;
                }

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
                    node.ImageIndex = imageIndex;
                    node.SelectedImageIndex = imageIndex;
                    node.Tag = entity;
                }
                if (entity.GetType() == typeof(OrganizationalUnit))
                {
                    GetView(search, (OrganizationalUnit)entity, parentNode);
                }

            }

        }

        //This is the function that adds property text to the list view / content field
        private void AddText(string name, string value)
        {
            ListViewItem lvi = new ListViewItem(new string[] { name, value});
            this.listView1.Items.Add(lvi);
        }

        //When tree-view item selected, fills the content list view with item properties
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Action == TreeViewAction.ByMouse)
            {
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
                    else AddText(property, "" + value);
                }

            }
        }

        //Search Button Event
        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            if (textBox1.Text.Replace(' ', '\0').Length < 1)
                GetView(Directory.GetRoot(connection), treeView1.Nodes);
            else GetView(textBox1.Text, Directory.GetRoot(connection), treeView1.Nodes);
        }

        private void EditUser(object sender, TreeNodeMouseClickEventArgs treeNodeMouseClickEventArgs)
        {
            new Thread(() =>
            {
                try
                {
                    Application.Run(new EditUser((User)treeNodeMouseClickEventArgs.Node.Tag, connection));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error cannot open that type \n" + e.Message);
                }
            }).Start();
        }
    }
}
