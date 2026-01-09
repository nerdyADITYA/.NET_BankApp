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

            // Logged-in user data
            Balance = Convert.ToInt32(Login.Balance);
            SAccountName = Login.AcName;
            SAccountNumber = Login.AcNumber;

            AmountLbl.Text = "₹" + Balance;
            UserNameLbl.Text = "Hello, " + SAccountName;

            string accNo = SAccountNumber;
            AcNumberLbl.Text = "**** **** **** " + accNo.Substring(accNo.Length - 4);

            GetAccounts();
            ShowTransactions();
            LoadIncomeExpense();
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
                "SELECT " +
                "CASE " +
                "   WHEN SenderAcNo = '{0}' THEN 'Sent' " +
                "   WHEN RecieverAcNo = '{0}' THEN 'Received' " +
                "END AS [Transaction Type], " +
                "CASE " +
                "   WHEN SenderAcNo = '{0}' THEN RecieverName " +
                "   ELSE SenderName " +
                "END AS [Account], " +
                "Amount AS Amount, " +
                "TrDate AS [Date], " +
                "Message " +
                "FROM TransactionTbl " +
                "WHERE SenderAcNo = '{0}' OR RecieverAcNo = '{0}' " +
                "ORDER BY TrDate DESC";

            query = string.Format(query, SAccountNumber);
            TransactionTbl.DataSource = Con.GetData(query);
        }

        // ================= INCOME / EXPENSE =================

        private void LoadIncomeExpense()
        {
            try
            {
                // EXPENSE (Money Sent)
                string expenseQuery =
                    "SELECT ISNULL(SUM(Amount),0) FROM TransactionTbl " +
                    "WHERE SenderAcNo = '{0}'";

                expenseQuery = string.Format(expenseQuery, SAccountNumber);
                DataTable expDt = Con.GetData(expenseQuery);
                int totalExpense = Convert.ToInt32(expDt.Rows[0][0]);
                ExpenseLbl.Text = "₹" + totalExpense;

                // INCOME (Money Received)
                string incomeQuery =
                    "SELECT ISNULL(SUM(Amount),0) FROM TransactionTbl " +
                    "WHERE RecieverAcNo = '{0}'";

                incomeQuery = string.Format(incomeQuery, SAccountNumber);
                DataTable incDt = Con.GetData(incomeQuery);
                int totalIncome = Convert.ToInt32(incDt.Rows[0][0]);
                IncomeLbl.Text = "₹" + totalIncome;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ================= LOGOUT =================

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
                LoadIncomeExpense();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
