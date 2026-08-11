using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetBuddy
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the Login Form first
            Login loginForm = new Login();
            DialogResult result = loginForm.ShowDialog();

            // If login is successful, open Dashboard
            if (result == DialogResult.OK)
            {
                Application.Run(new Dashboard()); 
            }
        }
    }
}
