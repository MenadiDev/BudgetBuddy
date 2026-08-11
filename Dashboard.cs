using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetBuddy
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();

           
            panelAddTransaction.BackColor = Color.MidnightBlue;
            panelAddTransaction.Cursor = Cursors.Hand;
            panelAddTransaction.Click += panelAddTransaction_Click;

            panelViewTransaction.BackColor = Color.Brown;
            panelViewTransaction.Cursor = Cursors.Hand;
            panelViewTransaction.Click += panelViewTransaction_Click;

            panelLogout.BackColor = Color.LightSeaGreen;
            panelLogout.Cursor = Cursors.Hand;
            panelLogout.Click += panelLogout_Click;

            
            AttachClickEvents(panelAddTransaction, panelAddTransaction_Click);
            AttachClickEvents(panelViewTransaction, panelViewTransaction_Click);
            AttachClickEvents(panelLogout, panelLogout_Click);

            this.Load += Dashboard_Load;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            UpdateFinancialSummary();
        }

        public void UpdateFinancialSummary()
        {
            double income = 0;
            double expenses = 0;

            string connectionString = "Data Source=BudgetBuddy.db";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Calculating Income
                string incomeQuery = "SELECT SUM(Amount) FROM Transactions WHERE Type='Income'";
                using (SQLiteCommand cmd = new SQLiteCommand(incomeQuery, conn))
                {
                    var result = cmd.ExecuteScalar();
                    income = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                }

                // Calculating Expenses
                string expenseQuery = "SELECT SUM(Amount) FROM Transactions WHERE Type='Expense'";
                using (SQLiteCommand cmd = new SQLiteCommand(expenseQuery, conn))
                {
                    var result = cmd.ExecuteScalar();
                    expenses = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                }
            }

            double balance = income - expenses;

            lblIncome.Text = $"Total Income: Rs. {income:N2}";
            lblExpenses.Text = $"Total Expenses: Rs. {expenses:N2}";
            lblBalance.Text = $"Balance: Rs. {balance:N2}";
        }

        private void panelAddTransaction_Click(object sender, EventArgs e)
        {
            AddTransactionForm addForm = new AddTransactionForm();
            addForm.ShowDialog();
            UpdateFinancialSummary();
        }

        private void panelViewTransaction_Click(object sender, EventArgs e)
        {
            ViewTransactionsForm viewForm = new ViewTransactionsForm(this); 
            viewForm.ShowDialog();
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

        private void lblExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit the page?",
                "Exit Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        
        private void AttachClickEvents(Panel panel, EventHandler handler)
        {
            foreach (Control control in panel.Controls)
            {
                control.Click += handler;
            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e) { }

        private void panel6_Paint(object sender, PaintEventArgs e) { }

        private void panelSummary_Paint(object sender, PaintEventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void panelLogout_Paint(object sender, PaintEventArgs e) { }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
