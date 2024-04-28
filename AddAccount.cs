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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Act4_EDP
{
    public partial class AddAccount : Form
    {
        public AddAccount()


        {
            InitializeComponent();
        }


        private bool IsUsernameExists(MySqlConnection connection, string username)
        {
            string query = "SELECT COUNT(*) FROM useraccounts WHERE username = @Username";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                // Open the connection if it's not already open
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // Execute the query and return true if count is greater than 0
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection

                    string username = emailbox.Text;

                    // Check for duplicate username
                    if (IsUsernameExists(connection, username))
                    {
                        MessageBox.Show("Username already exists. Please choose a different one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert the new user account into the database
                    string query = "INSERT INTO useraccounts (first_name, last_name, username, password, birthdate, rec_email) VALUES (@FirstName, @LastName, @Username, @Password, @BirthDate, @RecoveryEmail)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Retrieve user input
                        string first_name = fnamebox.Text;
                        string last_name = lnamebox.Text;
                        string password = passbox.Text;
                        DateTime birthDate = dob.Value;
                        string rec_mail = recbox.Text;

                        // Add parameters
                        command.Parameters.AddWithValue("@FirstName", first_name);
                        command.Parameters.AddWithValue("@LastName", last_name);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@BirthDate", birthDate);
                        command.Parameters.AddWithValue("@RecoveryEmail", rec_mail);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Hide();
                            var acc = new Accounts();
                            acc.Show();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create user account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}


     





