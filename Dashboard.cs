using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Act4_EDP
{
    public partial class Dashboard : Form
    {
        MySqlConnection sqlConn = new MySqlConnection();
        private int userID; // Variable to store the user ID
        public Dashboard(int userID)
        {
            InitializeComponent();
            this.userID = userID; // Store the user ID passed from the login form
            DisplayUserData(userID);

        }

        private void button2_Click_1(object sender, EventArgs e) // Logout button
        {
            // Display a message box with Yes and No options
            DialogResult result = MessageBox.Show("Do you want to log out?", "Log Out", MessageBoxButtons.OKCancel);

            // Check the result of the message box
            if (result == DialogResult.OK)
            {
                // Show the login form
                var loginForm = new Login();
                loginForm.Show();

                // Close the dashboard form
                this.Close();
            }
            else
            {
                // If user clicks Cancel, keep the dashboard form visible
                this.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();

                // Convert the int userID to a string before passing it to the Accounts constructor
                string userIDString = userID.ToString();

                this.Close();

                // Show the Accounts form and pass the current user ID
                Accounts gotoacc = new Accounts(userIDString);
                gotoacc.FormClosed += AccountsFormClosed; // Subscribe to the FormClosed event
                gotoacc.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Accounts: " + ex.Message);
            }
        }


        private void AccountsFormClosed(object sender, FormClosedEventArgs e)
        {
            // Update the user data when the Accounts form is closed
            DisplayUserData(userID);
            this.Show();
        }

        // Example method to display user-specific data based on the userID
        private void DisplayUserData(int userID)
        {
            string firstName = RetrieveFirstNameFromDatabase(userID);
            userDataLabel.Text = "Welcome, " + firstName;
        }

        private string RetrieveFirstNameFromDatabase(int userID)
        {
            string firstName = "";

            try
            {
                string myConnectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
                using (MySqlConnection conn = new MySqlConnection(myConnectionString))
                {
                    conn.Open();
                    string query = "SELECT first_name FROM useraccounts WHERE id = @userID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            firstName = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user's first name: " + ex.Message);
            }

            return firstName;
        }

        private void appointment_db_Click(object sender, EventArgs e)
        {
            Appointment AppStart = new Appointment();
            this.Hide();
            AppStart.Show();
        }

        private void consultation_db_Click(object sender, EventArgs e)
        {
            Consultation ConStart = new Consultation();
            this.Hide();
            ConStart.Show();
        }

        private void billingreport_db_Click(object sender, EventArgs e)
        {
            Billing AppStart = new Billing();
            this.Hide();
            AppStart.Show();
        }
    }

    // Accounts form
    
 
}






