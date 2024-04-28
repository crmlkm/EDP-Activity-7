using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Act4_EDP
{
    public partial class Billing : Form
    {
        private string connections = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
        public Billing()
        {
            InitializeComponent();

            LoadBillingData(); //load user data into the data grid
        }

        private void LoadBillingData()// load data in datagrid to view the appointment table
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }//for the data grid view

        private void Billing_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Select a Month";// show a placeholder in a comboBox1
            comboBox2.Text = "Select a Year";// show a placeholder in a comboBox1
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if a year is selected
            if (comboBox2.SelectedItem != null)
            {
                string selectedYear = comboBox2.SelectedItem.ToString();

                // Update the text box with the selected year
                textBox2.Text = selectedYear;

                // Filter data in the DataGridView based on the selected year
                FilterBillingData(selectedYear);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if a month is selected
            if (comboBox1.SelectedItem != null)
            {
                // Get the selected month
                string selectedMonth = comboBox1.SelectedItem.ToString();

                // Convert the selected month to its numerical representation
                string monthNumber = GetMonthNumber(selectedMonth);

                // Update the text box with the selected month
                textBox1.Text = monthNumber;

                // Filter data in the DataGridView based on the selected year and month
                FilterBillingData(textBox2.Text, monthNumber);
            }
        }

        private void FilterBillingData(string year, string month = "")
        {
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection

                    // Query to filter data based on year and month
                    string query = "SELECT * FROM billing WHERE YEAR(bill_date) = @year";

                    // If a month is provided, add it to the query
                    if (!string.IsNullOrEmpty(month))
                    {
                        query += " AND MONTH(bill_date) = @month";
                    }

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@year", year);
                        if (!string.IsNullOrEmpty(month))
                        {
                            command.Parameters.AddWithValue("@month", month);
                        }

                        // Create a DataAdapter to fetch filtered data from the database
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

        private string GetMonthNumber(string monthName)
        {
            // Dictionary to map month names to their numerical representations
            Dictionary<string, string> monthMap = new Dictionary<string, string>()
    {
        {"January", "01"},
        {"February", "02"},
        {"March", "03"},
        {"April", "04"},
        {"May", "05"},
        {"June", "06"},
        {"July", "07"},
        {"August", "08"},
        {"September", "09"},
        {"October", "10"},
        {"November", "11"},
        {"December", "12"}
    };

            // Try to get the numerical representation of the month
            if (monthMap.ContainsKey(monthName))
            {
                return monthMap[monthName];
            }
            else
            {
                return ""; // Return an empty string if the month name is invalid
            }
        }
        private void ComputePatientCount(string year)
        {
            string query = "SELECT COUNT(DISTINCT DATE(bill_date)) FROM billing WHERE YEAR(bill_date) = @year";
            using (MySqlConnection connection = new MySqlConnection(connections))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        numpatient.Text = result.ToString();
                    }
                }
            }
        }

        private void ComputeAppointmentCount(string year)
        {
            string query = "SELECT COUNT(*) FROM appointment WHERE YEAR(appointment_date) = @year";
            using (MySqlConnection connection = new MySqlConnection(connections))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        numapp.Text = result.ToString();
                    }
                }
            }
        }

        private void ComputeConsultationCount(string year)
        {
            string query = "SELECT COUNT(*) FROM medicalconsultation WHERE YEAR(consultation_date) = @year";
            using (MySqlConnection connection = new MySqlConnection(connections))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        numcon.Text = result.ToString();
                    }
                }
            }
        }

        private void ComputeBillingTotal(string year, string month = "")
        {
            string query = "SELECT SUM(total_amount) FROM billing WHERE YEAR(bill_date) = @year";

            if (!string.IsNullOrEmpty(month))
            {
                query += " AND MONTH(bill_date) = @month";
            }

            using (MySqlConnection connection = new MySqlConnection(connections))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    if (!string.IsNullOrEmpty(month))
                    {
                        command.Parameters.AddWithValue("@month", month);
                    }

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        numbill.Text = result.ToString();
                    }
                    else
                    {
                        numbill.Text = "0";
                    }
                }
            }
        }



        private void ComputeTotalAmount(string year)
        {
            string query = "SELECT SUM(total_amount) FROM billing WHERE YEAR(bill_date) = @year";
            using (MySqlConnection connection = new MySqlConnection(connections))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && !Convert.IsDBNull(result))
                    {
                        total.Text = result.ToString();
                    }
                    else
                    {
                        total.Text = "0";
                    }
                }
            }
        }


        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            string year = textBox2.Text;
            string month = textBox1.Text; // Assuming textBox1 contains the month value
            ComputePatientCount(year);
            ComputeAppointmentCount(year);
            ComputeConsultationCount(year);
            ComputeBillingTotal(year);
            ComputeTotalAmount(year);
        } //end of textbox results



        private void genrec_Click(object sender, EventArgs e)
        {
            // Create an instance of the Graphs form
            Graphs graphsForm = new Graphs();

            // Retrieve data from the billing table for the specified year
            DataTable billingData = GetBillingData(textBox2.Text, textBox1.Text);

            // Retrieve data from the medicalconsultation table for the specified year
            DataTable consultationData = GetConsultationData(textBox2.Text, textBox1.Text);

            // Pass the retrieved data and the selected year to the SetChartData method in the Graphs form
            graphsForm.SetChartData(billingData, consultationData, textBox2.Text);

            // Retrieve appointment data from the database
            DataTable appointmentData = GetAppointmentDataFromDatabase();

            // Pass the appointment data to the SetAppointmentChartData method in the Graphs form
            graphsForm.SetAppointmentChartData(appointmentData);

            // Retrieve total income data from the database
            DataTable totalIncomeData = TotalIncomeData();

            // Pass the retrieved data to the SetTotalIncomeData method in the Graphs form
            graphsForm.SetTotalIncomeData(totalIncomeData);

            // Call the method in Graphs form to update the yearTextBox
            graphsForm.UpdateYearTextBox(textBox2.Text);

            // Hide the current form instead of closing it
            this.Visible = false;

            // Show the Graphs form
            graphsForm.ShowDialog(); // Use ShowDialog to make the graphs form modal
            this.Visible = true; // Show the current form again after the graphs form is closed
        }


        private DataTable TotalIncomeData()
        {
            DataTable totalIncomeData = new DataTable();
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Write your SQL query to fetch total income data based on bill_date
                    // Here, we're selecting the year and the total amount from the billing table
                    // We're filtering the data for the years 2020-2025 and ordering by year in ascending order
                    string query = "SELECT YEAR(bill_date) AS Year, SUM(total_amount) AS TotalIncome " +
                                   "FROM billing " +
                                   "WHERE YEAR(bill_date) BETWEEN 2020 AND 2025 " +
                                   "GROUP BY YEAR(bill_date) " +
                                   "ORDER BY YEAR(bill_date) ASC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Fill the DataTable with total income data
                            adapter.Fill(totalIncomeData);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching total income data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalIncomeData;
        }



        private DataTable GetBillingData(string year, string month)
            {
                DataTable billingData = new DataTable();
                string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM billing WHERE YEAR(bill_date) = @year";
                        if (!string.IsNullOrEmpty(month))
                        {
                            query += " AND MONTH(bill_date) = @month";
                        }
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@year", year);
                            if (!string.IsNullOrEmpty(month))
                            {
                                command.Parameters.AddWithValue("@month", month);
                            }
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                adapter.Fill(billingData);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("An error occurred while fetching billing data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return billingData;
            }

        private DataTable GetConsultationData(string year, string month)
        {
            DataTable consultationData = new DataTable();
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM medicalconsultation WHERE YEAR(consultation_date) = @year";
                    if (!string.IsNullOrEmpty(month))
                    {
                        query += " AND MONTH(consultation_date) = @month";
                    }
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@year", year);
                        if (!string.IsNullOrEmpty(month))
                        {
                            command.Parameters.AddWithValue("@month", month);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(consultationData);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching consultation data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return consultationData;
        }

        // Method to retrieve appointment data from the database
        private DataTable GetAppointmentDataFromDatabase()
        {
            DataTable appointmentData = new DataTable();
            string connectionString = "server=localhost;uid=root;pwd=carmilo;database=clinic_db";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Write your SQL query to fetch appointment data based on appointment_date
                    // Here, we're selecting all appointments from the appointment table within the year range 2020-2025
                    string query = "SELECT * FROM appointment WHERE YEAR(appointment_date) BETWEEN 2020 AND 2025";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Fill the DataTable with appointment data
                            adapter.Fill(appointmentData);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching appointment data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return appointmentData;
        }

        private void genprint_Click(object sender, EventArgs e)
        {
            try
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                string templatePath = @"C:\Users\admin\Documents\BICOL UNIVERSITY\3RD YEAR\2ND SEM\IT 120 - EDP\Act 6\BILLING_REPORT.xlsx";
                FileInfo templateFile = new FileInfo(templatePath);

                using (ExcelPackage excelPackage = new ExcelPackage(templateFile))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sheet1"];

                    if (worksheet != null)
                    {
                        string year = textBox2.Text;

                        // Set the year value in cell B5
                        worksheet.Cells["B5"].Value = year;

                        // Set the number of patients in cell D5
                        worksheet.Cells["D5"].Value = numpatient.Text;

                        // Set other values accordingly
                        worksheet.Cells["E5"].Value = numapp.Text;
                        worksheet.Cells["F5"].Value = numcon.Text;
                        worksheet.Cells["G5"].Value = numbill.Text;
                        worksheet.Cells["H5"].Value = total.Text;

                        // Set the actual date today in cell H3
                        worksheet.Cells["H3"].Value = DateTime.Now.ToString("yyyy-MM-dd");

                        FileInfo outputFile = new FileInfo("BILLING_REPORT.xlsx");
                        excelPackage.SaveAs(outputFile);

                        MessageBox.Show("Invoice exported successfully.");

                        // Open the Excel file with the default application
                        Process.Start(outputFile.FullName);

                        // Insert images from a folder into Sheet2
                        if (InsertImagesFromFolder(excelPackage))
                        {
                            MessageBox.Show("Images inserted successfully in Sheet2.");
                        }
                        else
                        {
                            MessageBox.Show("No images found or error inserting images.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Worksheet 'Billing' does not exist in the template.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool InsertImagesFromFolder(ExcelPackage excelPackage)
        {
            try
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sheet2"];

                if (worksheet != null)
                {
                    string imagesFolderPath = @"C:\Users\admin\Documents\BICOL UNIVERSITY\3RD YEAR\2ND SEM\IT 120 - EDP\Act 6\";

                    string[] imageFiles = Directory.GetFiles(imagesFolderPath, "*.png");

                    int startRow = 2; // Start from row 2
                    int startColumn = 2; // Start from column B

                    foreach (string imageFile in imageFiles)
                    {
                        InsertImage(worksheet, imageFile, startRow, startColumn);

                        startColumn += 10; // Adjust this value as needed to leave space between images

                        // Reset column position if it exceeds the worksheet's column count
                        if (startColumn >= worksheet.Dimension?.Columns)
                        {
                            startColumn = 2; // Start from column B again
                            startRow += 20; // Move to the next row to avoid overlap
                        }
                    }

                    return true; // Images inserted successfully
                }
                else
                {
                    MessageBox.Show("Worksheet 'Sheet2' does not exist in the template.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting images: " + ex.Message);
                return false;
            }
        }


        private void InsertImage(ExcelWorksheet worksheet, string imagePath, int startRow, int startColumn)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    ExcelPicture picture = worksheet.Drawings.AddPicture(Path.GetFileName(imagePath), ms);
                    picture.SetPosition(startRow, 0, startColumn, 0);
                    picture.SetSize(100); // Set the size of the image (optional)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting image: " + ex.Message);
            }
        }


    }
}
 



