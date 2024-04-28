using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Act4_EDP
{
    public partial class ResetPass : Form
    {
        public ResetPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword = txtResetPass.Text;
            string confirmPassword = txtResetPassVerify.Text;

            // Check if the passwords match
            if (newPassword == confirmPassword)
            {
                try
                {
                    // MySqlConnection initialization
                    MySqlConnection conn;
                    string myConnectionString;
                    myConnectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
                    conn = new MySqlConnection(myConnectionString);
                    conn.Open();

                    // Update the password in the database
                    string updateSql = "UPDATE useraccounts SET Password = @newPassword WHERE id = @myid";
                    MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@myrec", txtResetPass.Text); // Assuming the email is in txtRecPass
                    updateCmd.ExecuteNonQuery();

                    // Close the connection
                    conn.Close();

                    // Show success message
                    MessageBox.Show("Password updated successfully!");

                    this.Hide(); // Hide the resetpass form
                    var loginForm = new Login(); // Assuming the login form is named Login
                    loginForm.Show(); // Show the login form
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error updating password: " + ex.Message);
                }
            }
            else
            {
                // Passwords don't match, show error message
                MessageBox.Show("Passwords don't match!");
            }
        }

       

public class DatabaseHelper
    {
        public string GetRecEmail()
        {
            string recEmail = null;
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Execute a query to retrieve the rec_email
                    string query = "SELECT rec_email FROM useraccounts WHERE id = @userId";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the rec_email from the reader
                            recEmail = reader.GetString("rec_email");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return recEmail;
        }
    }


    private void txtResetPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtResetPassVerify_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

/*
  if (txtResetPass.Text == txtResetPassVerify.Text)
            {
                SqlConnection conn = new SqlConnection("server=localhost;uid=root;pwd=carmilo;database=clinic_db");
                SqlCommand cmd = new SqlCommand();
            }
 */