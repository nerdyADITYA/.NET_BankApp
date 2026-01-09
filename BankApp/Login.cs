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
        public static string AcName;
        public static string AcNumber;
        public static int Balance;
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (RoleCb.SelectedIndex == -1 || UsernameTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Data !!!");
                return;
            }

            string selectedRole = RoleCb.Text.Trim().ToLower();

            string query =
                "SELECT UId, UName, UPassword, URole " +
                "FROM Users " +
                "WHERE UName = '{0}' AND UPassword = '{1}'";

            query = string.Format(query, UsernameTb.Text, PasswordTb.Text);
            DataTable dt = Con.GetData(query);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Wrong Username or Password");
                return;
            }

            string dbRole = dt.Rows[0]["URole"].ToString().Trim().ToLower();

            // 🔐 Role validation
            if (selectedRole != dbRole)
            {
                MessageBox.Show("Role does not match your account");
                return;
            }

            // ✅ ROLE MATCHED → REDIRECT
            if (dbRole == "admin")
            {
                Accounts obj = new Accounts();
                obj.Show();
                this.Hide();
            }
            else if (dbRole == "client")
            {
                // Load client account details
                string accQuery =
                    "SELECT AcNumber, AcName, Balance " +
                    "FROM AccountTbl WHERE AcName = '{0}'";

                accQuery = string.Format(accQuery, UsernameTb.Text);
                DataTable accDt = Con.GetData(accQuery);

                if (accDt.Rows.Count == 0)
                {
                    MessageBox.Show("Account not found for this user");
                    return;
                }

                AcNumber = accDt.Rows[0]["AcNumber"].ToString();
                AcName = accDt.Rows[0]["AcName"].ToString();
                Balance = Convert.ToInt32(accDt.Rows[0]["Balance"]);

                Dashboard obj = new Dashboard();
                obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid role assigned to user");
            }
        }


    }
}
