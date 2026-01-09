using System;
using System.Data;
using System.Windows.Forms;

namespace BankApp
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Con = new Functions();

            Balance = Convert.ToInt32(Login.Balance);
            AmountLbl.Text = "₹" + Balance;
            UserNameLbl.Text = "Hello, " + Login.AcName;

            SAccountName = Login.AcName;
            SAccountNumber = Login.AcNumber;

            string accNo = Login.AcNumber;
            AcNumberLbl.Text = "**** **** **** " + accNo.Substring(accNo.Length - 4);

            GetAccounts();
            ShowTransactions();
        }

        Functions Con;
        int Balance;
        string SAccountNumber = "";
        string SAccountName = "";

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        // ================= ACCOUNTS =================

        private void GetAccounts()
        {
            string query = "SELECT AcNumber, AcName FROM AccountTbl";
            DataTable dt = Con.GetData(query);

            AccountCb.DataSource = dt;
            AccountCb.DisplayMember = "AcName";
            AccountCb.ValueMember = "AcNumber";
            AccountCb.SelectedIndex = -1;
        }

        // ================= TRANSACTIONS =================

        private void ShowTransactions()
        {
            string query =
                "SELECT RecieverName AS Reciever, Amount AS [Money Sent], " +
                "TrType AS [Operation], TrDate AS [Date], Message " +
                "FROM TransactionTbl";

            TransactionTbl.DataSource = Con.GetData(query);
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Hide();
        }

        private void ClearFields()
        {
            AmountTb.Text = "";
            MessageTb.Text = "";
            AccountCb.SelectedIndex = -1;
        }

        // ================= BALANCE UPDATES =================

        private void UpdateBalanceSender(int newBalance)
        {
            string query =
                $"UPDATE AccountTbl SET Balance = {newBalance} WHERE AcNumber = '{SAccountNumber}'";

            Con.SetData(query);
            Balance = newBalance;
            AmountLbl.Text = "₹" + Balance;
        }

        private void UpdateBalanceReceiver(int amount)
        {
            string receiverAccNo = AccountCb.SelectedValue.ToString();

            string selectQuery =
                $"SELECT Balance FROM AccountTbl WHERE AcNumber = '{receiverAccNo}'";

            DataTable dt = Con.GetData(selectQuery);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Receiver account not found");
                return;
            }

            int currentBalance = Convert.ToInt32(dt.Rows[0]["Balance"]);
            int newBalance = currentBalance + amount;

            string updateQuery =
                $"UPDATE AccountTbl SET Balance = {newBalance} WHERE AcNumber = '{receiverAccNo}'";

            Con.SetData(updateQuery);
        }

        // ================= TRANSFER =================

        private void TransferBtn_Click(object sender, EventArgs e)
        {
            if (AmountTb.Text == "" || MessageTb.Text == "" || AccountCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Data !!!");
                return;
            }

            int amount = Convert.ToInt32(AmountTb.Text);

            if (amount <= 0)
            {
                MessageBox.Show("Invalid Amount");
                return;
            }

            if (amount > Balance)
            {
                MessageBox.Show("Insufficient Balance");
                return;
            }

            try
            {
                int newSenderBalance = Balance - amount;

                string insertQuery =
                    "INSERT INTO TransactionTbl VALUES " +
                    "('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}')";

                insertQuery = string.Format(
                    insertQuery,
                    SAccountNumber,
                    SAccountName,
                    AccountCb.SelectedValue.ToString(),
                    AccountCb.Text,
                    amount,
                    newSenderBalance,
                    "Transfer",
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    MessageTb.Text
                );

                Con.SetData(insertQuery);

                UpdateBalanceSender(newSenderBalance);
                UpdateBalanceReceiver(amount);

                MessageBox.Show("Transfer Successful !!!");
                ClearFields();
                ShowTransactions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
