using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BudgetBuddy
{
    public partial class ViewTransactionsForm : Form
    {
        private DataTable transactionTable;
        private Dashboard _dashboard;

        public ViewTransactionsForm(Dashboard dashboard)
        {
            InitializeComponent();
            _dashboard = dashboard;

            panelAddTransaction.Click += panelAddTransaction_Click;
            panelDashboard.Click += panelDashboard_Click;
            btnExit.Click += btnExit_Click;

            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;
        }

        private void ViewTransactionsForm_Load(object sender, EventArgs e)
        {
            LoadTransactions();
            cmbSearchBy.Items.AddRange(new string[] { "Category", "Amount", "Description", "Date" });
            cmbSearchBy.SelectedIndex = 0;
        }

        public void LoadTransactions()
        {
            using (SQLiteConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Transactions";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                transactionTable = new DataTable();
                adapter.Fill(transactionTable);
                dataGridView1.DataSource = transactionTable;
            }
        }

        private void panelAddTransaction_Click(object sender, EventArgs e)
        {
            AddTransactionForm addForm = new AddTransactionForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadTransactions();       
                _dashboard.UpdateFinancialSummary(); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int transactionId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["TransactionID"].Value);
                DialogResult confirm = MessageBox.Show(
                    "Are you sure you want to delete this transaction?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    using (SQLiteConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Transactions WHERE TransactionID = @id";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", transactionId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadTransactions();                 
                    _dashboard.UpdateFinancialSummary(); 
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction to delete.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int transactionId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["TransactionID"].Value);

                EditTransactionForm editForm = new EditTransactionForm(transactionId, _dashboard, this);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadTransactions(); 
                }

            }
            else
            {
                MessageBox.Show("Please select a transaction to edit.");
            }
        }

        private void panelDashboard_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            string selectedColumn = cmbSearchBy.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(selectedColumn))
            {
                MessageBox.Show("Please enter a search term and select a field.");
                return;
            }

            try
            {
                if (selectedColumn == "Amount")
                {
                    transactionTable.DefaultView.RowFilter = $"{selectedColumn} = {searchTerm}";
                }
                else
                {
                    string escaped = searchTerm.Replace("'", "''");
                    transactionTable.DefaultView.RowFilter = $"{selectedColumn} LIKE '%{escaped}%'";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying search filter: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (transactionTable != null)
                transactionTable.DefaultView.RowFilter = string.Empty;
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
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            
        }

    }
}
