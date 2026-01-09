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
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            UsersList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UsersList.MultiSelect = false;
            Con = new Functions();
            ShowUsers();
        }

        Functions Con;

        private void ShowUsers()
        {
            string Query = "Select * from Users";
            UsersList.DataSource = Con.GetData(Query);
        }

        private void clearFields()
        {
            UNameTb.Text = "";
            UPasswordTb.Text = "";
            UPhoneTb.Text = "";
            URoleTb.Text = "";
            UGenderCb.SelectedIndex = -1;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (UNameTb.Text == "" || UPasswordTb.Text == "" || UPhoneTb.Text == "" || URoleTb.Text == "" || UGenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Data !!!");
            }
            else
            {
                try
                {
                    string UName = UNameTb.Text;
                    string UPhone = UPhoneTb.Text;
                    string UGender = UGenderCb.SelectedItem.ToString();
                    string UPassword = UPasswordTb.Text;
                    string URole = URoleTb.Text;

                    string Query = "INSERT INTO Users VALUES('{0}','{1}','{2}','{3}','{4}')";
                    Query = string.Format(Query, UName, UPhone, UGender, UPassword, URole);
                    Con.SetData(Query);
                    MessageBox.Show("User Added !!!");
                    ShowUsers();
                    clearFields();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        string key = "";

        private void UsersList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = UsersList.Rows[e.RowIndex];
            UNameTb.Text = row.Cells[1].Value.ToString();
            UPhoneTb.Text = row.Cells[2].Value.ToString();
            UGenderCb.SelectedItem = row.Cells[3].Value.ToString();
            UPasswordTb.Text = row.Cells[4].Value.ToString();
            URoleTb.Text = row.Cells[5].Value.ToString();

            key = row.Cells["UId"].Value.ToString();

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (key == "")
            {
                MessageBox.Show("Please select an account first");
                return;
            }
            else
            {
                try
                {

                    string query = "UPDATE Users SET Uname = '{0}',UPhone = '{1}',UGender = '{2}',UPassword = '{3}',URole = '{4}' WHERE UId = '{5}'";
                    query = string.Format(query, UNameTb.Text, UPhoneTb.Text, UGenderCb.SelectedItem.ToString(), UPasswordTb.Text, URoleTb.Text, key);
                    Con.SetData(query);
                    MessageBox.Show("Record Updated successfully!!!");
                    ShowUsers();
                    clearFields();
                    key = "";
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (UNameTb.Text == "" || UPasswordTb.Text == "" || UPhoneTb.Text == "" || URoleTb.Text == "" || UGenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Data !!!");
            }
            else
            {
                try
                {
                    string Query = "DELETE FROM Users WHERE UId='{0}'";
                    Query = string.Format(Query, key);
                    Con.SetData(Query);
                    MessageBox.Show("Account Deleted!!!");
                    ShowUsers();
                    clearFields();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void AccountsBtn_Click(object sender, EventArgs e)
        {
            Accounts obj = new Accounts();
            obj.Show();
            this.Hide();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Hide();
        }
    }
}
