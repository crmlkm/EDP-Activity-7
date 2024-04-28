using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Act4_EDP
{
    public partial class Accounts : Form
    {
        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();
        String sqlQuery;
        MySqlDataAdapter DtA = new MySqlDataAdapter();
        MySqlDataReader sqlRd;

        public Accounts()
        {
            InitializeComponent();

            // Call the method to load data into the DataGridView
            LoadUserData();
        }

        // Method to load data into the DataGridView
        private void LoadUserData()
        {
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection

                    // Query to select all columns from the useraccounts table
                    string query = "SELECT * FROM useraccounts";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Create a DataAdapter to fetch data from the database
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the fetched data
                            System.Data.DataTable dataTable = new System.Data.DataTable();

                            // Fill the DataTable with data from the DataAdapter
                            adapter.Fill(dataTable);

                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Optionally, handle CellContentClick event if needed
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click event if needed
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Convert the string userID to an int
                int userIDInt = Convert.ToInt32(userID);

                // Show the Dashboard form and pass the user ID
                Dashboard godash = new Dashboard(userIDInt);
                godash.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Dashboard: " + ex.Message);
            }
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

        private void add_b_Click(object sender, EventArgs e)   //add the user 
        {

            String ConnectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";//connect to mySql database

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
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
                    string query = "INSERT INTO useraccounts (id, first_name, last_name, username, password, birthdate, rec_email) VALUES (@id, @FirstName, @LastName, @Username, @Password, @BirthDate, @RecoveryEmail)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Retrieve user input
                        string id = txtid.Text;
                        string first_name = fnamebox.Text;
                        string last_name = lnamebox.Text;
                        string password = passbox.Text;
                        DateTime birthDate = dob.Value;
                        string rec_mail = recbox.Text;

                        // Add parameters
                        command.Parameters.AddWithValue("@id", id);
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

        }//end of add user


        private void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear all textboxes in the panel
                foreach (Control pindot in panelinputs.Controls)
                {
                    if (pindot is TextBox)
                    {
                        ((TextBox)pindot).Clear();
                    }
                }

                // Clear the txtsearch textbox
                txtsearch.Clear();

                // Reset the DateTimePicker named dob to the current time
                dob.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//end of clear

        private void deletebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the user ID from the selected row in the DataGridView
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Get the user ID from the first selected row
                    int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                    // Establish connection to the database
                    using (MySqlConnection sqlConn = new MySqlConnection("server=localhost;uid=root;pwd=carmilo;database=clinic_db"))
                    {
                        // Open the connection
                        sqlConn.Open();

                        // Prepare the DELETE command
                        using (MySqlCommand sqlCmd = new MySqlCommand("DELETE FROM clinic_db.useraccounts WHERE id = @myid", sqlConn))
                        {
                            // Add the user ID parameter
                            sqlCmd.Parameters.AddWithValue("@myid", userID);

                            // Execute the command
                            int rowsAffected = sqlCmd.ExecuteNonQuery();

                            // Close the connection
                            sqlConn.Close();

                            // Check if any rows were affected
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Row deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("No rows were deleted.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        //end delete button

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtid.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                fnamebox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                lnamebox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                emailbox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                passbox.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                recbox.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

                // Convert the cell value to DateTime before assigning it to the DateTimePicker
                if (DateTime.TryParse(dataGridView1.SelectedRows[0].Cells[6].Value.ToString(), out DateTime dobValue))
                {
                    dob.Value = dobValue;
                }
                else
                {
                    // Handle the case where the date cannot be parsed
                    MessageBox.Show("Invalid date format in the DataGridView.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConn.ConnectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
                sqlConn.Open();

                // Prepare the SQL command with a parameterized query
                sqlQuery = "SELECT * FROM clinic_db.useraccounts WHERE id = @id";
                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);

                // Add the parameter to the SQL command
                sqlCmd.Parameters.AddWithValue("@id", txtsearch.Text);

                sqlRd = sqlCmd.ExecuteReader();

                if (sqlRd.Read())
                {
                    txtid.Text = sqlRd.GetInt32("id").ToString(); // Use GetInt32 to retrieve an integer value
                    fnamebox.Text = sqlRd.GetString("first_name");
                    lnamebox.Text = sqlRd.GetString("last_name");
                    emailbox.Text = sqlRd.GetString("username");
                    passbox.Text = sqlRd.GetString("password");
                    recbox.Text = sqlRd.GetString("rec_email");

                    // Get the index of the "birthdate" column
                    int dobIndex = sqlRd.GetOrdinal("birthdate");

                    // Retrieve the value of the "birthdate" column using the index
                    DateTime dobValue = sqlRd.GetDateTime(dobIndex);
                    dob.Value = dobValue;

                    // Close the connection
                    sqlConn.Close();
                }
                else
                {
                    MessageBox.Show("No Data Found", "MySql Connector", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Close the connection
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//end of search

        private void editbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Open connection
                sqlConn.ConnectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
                sqlConn.Open();

                // Prepare the SQL command with a parameterized query
                sqlQuery = "SELECT * FROM clinic_db.useraccounts WHERE id = @id";
                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);

                // Add the parameter to the SQL command
                sqlCmd.Parameters.AddWithValue("@id", txtsearch.Text);

                sqlRd = sqlCmd.ExecuteReader();

                if (sqlRd.Read())
                {
                    txtid.ReadOnly = false;
                    fnamebox.ReadOnly = false;
                    lnamebox.ReadOnly = false;
                    emailbox.ReadOnly = false;
                    passbox.ReadOnly = false;
                    recbox.ReadOnly = false;
                    dob.Enabled = true;

                    txtid.Text = sqlRd["id"].ToString(); // Retrieve ID as string
                    fnamebox.Text = sqlRd["first_name"].ToString(); // Retrieve first_name as string
                    lnamebox.Text = sqlRd["last_name"].ToString(); // Retrieve last_name as string
                    emailbox.Text = sqlRd["username"].ToString(); // Retrieve username as string
                    passbox.Text = sqlRd["password"].ToString(); // Retrieve password as string
                    recbox.Text = sqlRd["rec_email"].ToString(); // Retrieve rec_email as string

                    dob.Value = sqlRd.GetDateTime(sqlRd.GetOrdinal("birthdate")); // Retrieve birthdate as DateTime

                    // Close the connection
                    sqlConn.Close();
                }
                else
                {
                    MessageBox.Show("No Data Found", "MySql Connector", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Ensure the SqlDataReader is closed
                if (sqlRd != null && !sqlRd.IsClosed)
                {
                    sqlRd.Close();
                }

                // Ensure the connection is closed
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
        }


        private void savebtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Open connection
                sqlConn.Open();

                // Prepare the SQL command with a parameterized query
                sqlQuery = "UPDATE clinic_db.useraccounts SET first_name = @first_name, last_name = @last_name, username = @username, password = @password, rec_email = @rec_email, birthdate = @birthdate WHERE id = @id";
                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);

                // Add parameters
                sqlCmd.Parameters.AddWithValue("@first_name", fnamebox.Text);
                sqlCmd.Parameters.AddWithValue("@last_name", lnamebox.Text);
                sqlCmd.Parameters.AddWithValue("@username", emailbox.Text);
                sqlCmd.Parameters.AddWithValue("@password", passbox.Text);
                sqlCmd.Parameters.AddWithValue("@rec_email", recbox.Text);
                sqlCmd.Parameters.AddWithValue("@birthdate", dob.Value);
                sqlCmd.Parameters.AddWithValue("@id", txtid.Text);

                // Execute command
                int rowsAffected = sqlCmd.ExecuteNonQuery();

                // Close the connection
                sqlConn.Close();

                // Check if any rows were affected
                if (rowsAffected > 0)
                {
                    MessageBox.Show("User data updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the form
                    RefreshForm();

                    // Clear textboxes
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("No changes were made.", "Update Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Method to refresh the form
        private void RefreshForm()
        {
            Accounts form = new Accounts();
            form.Show();
        }

        // Method to clear textboxes
        private void ClearTextBoxes()
        {
            txtid.Clear();
            fnamebox.Clear();
            lnamebox.Clear();
            emailbox.Clear();
            passbox.Clear();
            recbox.Clear();
        }

        private string userID; // Variable to store the user ID

        // Method to display the welcome message with the user's name
        // Method to display the welcome message with the user's name
        private void DisplayWelcomeMessage(string userId)
        {
            string firstName = GetFirstName(userId);
            if (!string.IsNullOrEmpty(firstName))
            {
                // Display the welcome message in a label
                welcomeLabel.Text = "Welcome, " + firstName;
            }
            else
            {
                // Handle case where no first name is retrieved
                MessageBox.Show("Error: User's first name not found.");
            }
        }

        // Method to get the first name of the user using the user ID
        private string GetFirstName(string userID)
        {
            string firstName = ""; // Initialize the variable to store the first name
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
                            firstName = result.ToString(); // Retrieve the first name
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
        public Accounts(string id)
        {
            InitializeComponent();
            this.userID = id; // Store the user ID passed from the dashboard
            DisplayWelcomeMessage(userID);

            // Call the method to load data into the DataGridView
            LoadUserData();
        }
    }

}
    











