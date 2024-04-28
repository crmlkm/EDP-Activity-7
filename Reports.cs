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
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();

            LoadUserData();
        }

        private void LoadUserData()
        {
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection

                    // Query to select all columns from the useraccounts table
                    string query = "SELECT * FROM billing";

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

        private void Reports_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
