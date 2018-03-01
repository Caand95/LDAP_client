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
    public partial class FormConnect : Form
    {
        public FormConnect()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Connection connection = new Connection(domainName.Text, username.Text, password.Text);
                this.Hide();
                Browser browser = new Browser(connection);
                new Thread(() => { Application.Run(browser); }).Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to connect to " + domainName.Text + "\nError: " + ex, "Connection Error!");
            }
            
        }
    }
}
