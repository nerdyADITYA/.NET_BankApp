using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Con = new Functions();
        }
        Functions Con;
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (RoleCb.SelectedIndex == -1 || UsernameTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Data !!!");
            }
            else if (RoleCb.SelectedIndex == 0)
            {
                string Query = "select UId,UName,UPassword from Users where UName='{0}' and UPassword='{1}'";
                Query = string.Format(Query, UsernameTb.Text, PasswordTb.Text);
                DataTable dt = Con.GetData(Query);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Wrong Username or Password");
                }
                else
                {
                    Accounts obj = new Accounts();
                    obj.Show();
                    this.Hide();
                }
            }
            else
            {
                string Query = "select AcNumber,AcName,SecretCode from AccountTbl where AcName='{0}' and SecretCode='{1}'";
                Query = string.Format(Query, UsernameTb.Text, PasswordTb.Text);
                DataTable dt = Con.GetData(Query);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Wrong Username or Password");
                }
                else
                {
                    Dashboard obj = new Dashboard();
                    obj.Show();
                    this.Hide();
                }
            }
        }
    }
}
