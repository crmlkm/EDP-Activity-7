using MySql.Data.MySqlClient;
using OfficeOpenXml.Drawing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Act4_EDP
{
    public partial class Graphs : Form
    {
        public Graphs()
        {
            InitializeComponent();
        }


        public void SetChartData(DataTable billingData, DataTable consultationData, string year)
        {
            // Clear existing data in the chart
            chart1.Series["Patients"].Points.Clear();
            chart2.Series["Consultation"].Points.Clear();

            // Initialize arrays to hold patient counts and consultation counts for each month
            int[] patientCounts = new int[12];
            int[] consultationCounts = new int[12];

            // Iterate through the billing data to count patients for each month
            foreach (DataRow row in billingData.Rows)
            {
                int month = Convert.ToDateTime(row["bill_date"]).Month;
                patientCounts[month - 1]++;
            }

            // Iterate through the consultation data to count consultations for each month
            foreach (DataRow row in consultationData.Rows)
            {
                int month = Convert.ToDateTime(row["consultation_date"]).Month;
                consultationCounts[month - 1]++;
            }

            // Add data points to the Patients chart for each month
            for (int i = 0; i < 12; i++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1);
                chart1.Series["Patients"].Points.AddXY(monthName, patientCounts[i]);
            }

            // Add data points to the Consultations chart for each month
            for (int i = 0; i < 12; i++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1);
                chart2.Series["Consultation"].Points.AddXY(monthName, consultationCounts[i]);
            }

            // Set chart titles and labels
            chart1.Titles.Clear();
            chart1.Titles.Add("Patient Count for " + year);
            chart1.ChartAreas[0].AxisX.Title = "Month";
            chart1.ChartAreas[0].AxisY.Title = "Patients";

            chart2.Titles.Clear();
            chart2.Titles.Add("Consultation Count for " + year);
            chart2.ChartAreas[0].AxisX.Title = "Month";
            chart2.ChartAreas[0].AxisY.Title = "Consultations";
        }

        // Method to set the chart data for appointments
        public void SetAppointmentChartData(DataTable appointmentData)
        {
            // Clear existing data in the chart
            chart3.Series.Clear();
            chart3.Legends.Clear();

            // Add a legend
            chart3.Legends.Add("Legend1");

            // Add a series for appointments
            Series series = chart3.Series.Add("Appointments");
            series.ChartType = SeriesChartType.Pie;

            // Dictionary to store appointment counts by year
            Dictionary<string, int> appointmentCountsByYear = new Dictionary<string, int>();

            // Add data points to the series based on the retrieved data
            foreach (DataRow row in appointmentData.Rows)
            {
                // Extract the year from the appointment date
                DateTime appointmentDate = Convert.ToDateTime(row["appointment_date"]);
                string year = appointmentDate.Year.ToString();

                // Increment the appointment count for the corresponding year
                if (appointmentCountsByYear.ContainsKey(year))
                {
                    appointmentCountsByYear[year]++;
                }
                else
                {
                    appointmentCountsByYear[year] = 1;
                }
            }

            // Add data points to the series for each year
            foreach (var kvp in appointmentCountsByYear)
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }

            // Set chart title
            chart3.Titles.Clear();
            chart3.Titles.Add("Appointment Count by Year");

            // Set legend
            series.Legend = "Legend1";

        }

        public void SetTotalIncomeData(DataTable data)
        {
            // Clear existing data in the chart
            chart4.Series.Clear();
            chart4.Titles.Clear();

            // Add a new series for total income
            Series series = chart4.Series.Add("Total Income");
            series.ChartType = SeriesChartType.Line;

            // Add data points to the series
            foreach (DataRow row in data.Rows)
            {
                int year = Convert.ToInt32(row["Year"]);
                double totalIncome = Convert.ToDouble(row["TotalIncome"]);
                series.Points.AddXY(year.ToString(), totalIncome);
            }

            // Set chart title and axis labels
            chart4.Titles.Add("Total Income Over Years");
            chart4.ChartAreas[0].AxisX.Title = "Year";
            chart4.ChartAreas[0].AxisY.Title = "Total Income";
        }



        private void print_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the base file path where the images will be saved
                string basePath = @"C:\Users\admin\Documents\BICOL UNIVERSITY\3RD YEAR\2ND SEM\IT 120 - EDP\Act 6\";

                // Save each chart as an image
                SaveChartImage(chart1, basePath, "Chart1.png");
                SaveChartImage(chart2, basePath, "Chart2.png");
                SaveChartImage(chart3, basePath, "Chart3.png");
                SaveChartImage(chart4, basePath, "Chart4.png");

                // Show a message box indicating successful image save
                MessageBox.Show("Images saved to the folder.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Show a message box if there's an error saving the images
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveChartImage(Chart chart, string basePath, string fileName)
        {
            string filePath = Path.Combine(basePath, fileName);

            // Check if the file already exists
            if (File.Exists(filePath))
            {
                // If the file exists, find a unique name by appending (1), (2), etc.
                string directory = Path.GetDirectoryName(filePath);
                string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                int count = 1;

                // Loop until a unique file name is found
                do
                {
                    filePath = Path.Combine(directory, $"{fileNameOnly} ({count}){extension}");
                    count++;
                }
                while (File.Exists(filePath));
            }

            // Save the chart image with the unique file path
            chart.SaveImage(filePath, ChartImageFormat.Png);
        }





        private void button1_Click(object sender, EventArgs e)
        {
            Billing bill = new Billing();
            this.Close();
            bill.Show();
        }

        

        public void UpdateYearTextBox(string year)
        {
            // Set the text of yeartextBox to the provided year
            yeartextBox.Text = year;
        }

       
    }

}

