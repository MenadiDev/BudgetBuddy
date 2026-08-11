using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace BudgetBuddy
{
    public partial class EditTransactionForm : Form
    {
        private int transactionId;
        private ViewTransactionsForm _viewForm;
        private Dashboard _dashboard;

        public EditTransactionForm(int id, Dashboard dashboard, ViewTransactionsForm viewForm)
        {
            InitializeComponent();
            transactionId = id;
            _dashboard = dashboard;
            _viewForm = viewForm;

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelDashboard.Click += panelDashboard_Click;
            panelViewTransaction.Click += panelViewTransaction_Click;
            panelLogout.Click += panelLogout_Click;
            btnExit.Click += btnExit_Click;
        }


        private void EditTransactionForm_Load(object sender, EventArgs e)
        {
            cmbType.Items.Clear();
            cmbType.Items.Add("Income");
            cmbType.Items.Add("Expense");

            LoadTransactionDetails();
        }

        private void LoadTransactionDetails()
        {
            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Transactions WHERE TransactionID = @TransactionID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", transactionId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            datePicker.Value = Convert.ToDateTime(reader["Date"]);
                            cmbCategory.Text = reader["Category"].ToString();
                            cmbType.Text = reader["Type"].ToString();
                            txtAmount.Text = reader["Amount"].ToString();
                            txtDescription.Text = reader["Note"].ToString();
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                using (SQLiteConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string updateQuery = @"
                        UPDATE Transactions 
                        SET Date = @date, Category = @category, Type = @type, Amount = @amount, Note = @description 
                        WHERE TransactionID = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@date", datePicker.Value);
                        cmd.Parameters.AddWithValue("@category", cmbCategory.Text);
                        cmd.Parameters.AddWithValue("@type", cmbType.Text);
                        cmd.Parameters.AddWithValue("@amount", Convert.ToDecimal(txtAmount.Text));
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@id", transactionId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Transaction updated successfully!");
                
                _dashboard.UpdateFinancialSummary();
                _viewForm.LoadTransactions();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(cmbCategory.Text) ||
                string.IsNullOrWhiteSpace(cmbType.Text) ||
                (cmbType.Text != "Income" && cmbType.Text != "Expense") ||
                string.IsNullOrWhiteSpace(txtAmount.Text) ||
                !decimal.TryParse(txtAmount.Text, out _))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void panelDashboard_Click(object sender, EventArgs e)
        {
            _dashboard.Show();
            this.Close();
        }

        private void panelViewTransaction_Click(object sender, EventArgs e)
        {
            _viewForm.Show(); 
            this.Close();
        }

        private void panelLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
               "Are you sure you want to log out?",
               "Logout Confirmation",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

       
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}






