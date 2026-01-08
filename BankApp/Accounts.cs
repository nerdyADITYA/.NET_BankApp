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
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();
            AccountsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AccountsList.MultiSelect = false;
            Con = new Functions();
            ShowAccounts();
        }

        Functions Con;

        private void ShowAccounts()
        {
            string Query = "Select * from AccountTbl";
            AccountsList.DataSource = Con.GetData(Query);
        }

        private void ClearFields()
        {
            AcNameTb.Text = "";
            AcNumberTb.Text = "";
            AcPhoneTb.Text = "";
            AcBalanceTb.Text = "";
            AcAddressTb.Text = "";
            SecretCodeTb.Text = "";
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (AcNameTb.Text == "" || AcNumberTb.Text == "" || AcPhoneTb.Text == "" || AcBalanceTb.Text == "" || AcAddressTb.Text == "" || SecretCodeTb.Text == "")
            {
                MessageBox.Show("Missing Data !!!");
            }
            else
            {
                try
                {
                    string AcNumber = AcNumberTb.Text;
                    int Balance = Convert.ToInt32(AcBalanceTb.Text);
                    string AcName = AcNameTb.Text;
                    string SecretCode = SecretCodeTb.Text;
                    string AcPhone = AcPhoneTb.Text;
                    string AcAddress = AcAddressTb.Text;
                    string Query = "insert into AccountTbl values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}')";

                    Query = string.Format(Query, AcNumber, DateTime.Today.Date.ToString(), Balance, AcName, SecretCode, AcPhone, AcAddress);
                    Con.SetData(Query);
                    MessageBox.Show("Account Added !!!");
                    ShowAccounts();
                    ClearFields();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        string key = "";

        private void AccountsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Prevent header click crash
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = AccountsList.Rows[e.RowIndex];

            AcNumberTb.Text = row.Cells[0].Value.ToString();
            AcBalanceTb.Text = row.Cells[2].Value.ToString();
            AcNameTb.Text = row.Cells[3].Value.ToString();
            SecretCodeTb.Text = row.Cells[4].Value.ToString();
            AcPhoneTb.Text = row.Cells[5].Value.ToString();
            AcAddressTb.Text = row.Cells[6].Value.ToString();

            key = row.Cells["AcNumber"].Value.ToString();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (key == "")
            {
                MessageBox.Show("Please select an account first");
                return;
            }

            try
            {
                string query =
                    "UPDATE AccountTbl SET " +
                    "Balance = '{0}', " +
                    "AcName = '{1}', " +
                    "AcPhone = '{2}', " +
                    "SecretCode = '{3}', " +
                    "AcAddress = '{4}' " +
                    "WHERE AcNumber = '{5}'";

                query = string.Format(
                    query,
                    AcBalanceTb.Text,
                    AcNameTb.Text,
                    AcPhoneTb.Text,
                    SecretCodeTb.Text,
                    AcAddressTb.Text,
                    key
                );

                Con.SetData(query);

                MessageBox.Show("Record Updated successfully!!!");
                ShowAccounts();
                ClearFields();
                key = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (AcNameTb.Text == "" || AcNumberTb.Text == "" || AcPhoneTb.Text == "" || AcBalanceTb.Text == "" || AcAddressTb.Text == "" || SecretCodeTb.Text == "")
            {
                MessageBox.Show("Missing Data !!!");
            }
            else
            {
                try
                {
                    string AcNumber = AcNumberTb.Text;
                    string Query = "DELETE FROM AccountTbl WHERE AcNumber='{0}'";
                    Query = string.Format(Query, AcNumber);
                    Con.SetData(Query);
                    MessageBox.Show("Account Deleted!!!");
                    ShowAccounts();
                    ClearFields();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
