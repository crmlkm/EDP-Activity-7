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
    public partial class Consultation : Form
    {
        public Consultation()
        {
            InitializeComponent();

            LoadConsultationData(); //load user data into the data grid
        }

        private void LoadConsultationData()// load data in datagrid to view the appointment table
        {
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection
                            
                    // Query to select all columns from the useraccounts table
                    string query = "SELECT * FROM medicalconsultation";

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
