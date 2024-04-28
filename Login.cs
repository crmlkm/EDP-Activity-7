using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Act4_EDP
{
    public partial class Login : Form
    {
        public object GlobalVariables { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Will prevent the form from being minimized
            MinimizeBox = false;
            MaximizeBox = false;

        }


        private void cancel_b_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tc_checkbox_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void login_b_Click(object sender, EventArgs e)
        {
            string username = this.usernamebox.Text;
            string password = this.passbox.Text;

            int userID = 0; // Variable to store the user ID

            {
                MySql.Data.MySqlClient.MySqlConnection conn;
                string myConnectionString;
                myConnectionString = "server=localhost;uid=root;" +
                 "pwd=carmilo;database=clinic_db";
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                    conn.Open();
                    string sql = "SELECT id FROM UserAccounts WHERE username = @myuser AND password = @mypass"; // Modify the query to retrieve the user ID
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@myuser", username);
                    cmd.Parameters.AddWithValue("@mypass", password);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out userID))
                    {
                        // Open the Dashboard form and pass the user ID as a parameter
                        Dashboard DashStart = new Dashboard(userID);
                        this.Close();
                        DashStart.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username and Password");
                    }
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Connection Failed");
                            break;
                        case 1045:
                            MessageBox.Show("Invalid Username or Password, Please Try Again");
                            break;
                    }
                }
            }
        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void text2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void CheckConnection_Click(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;
            myConnectionString = "server=localhost;uid=root;" +
             "pwd=carmilo;database=clinic_db";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                MessageBox.Show("Connected");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Forgot = new PasswordRec();
            this.Hide();
            Forgot.Show();
        }

        private void usernamebox_TextChanged(object sender, EventArgs e)
        {

        }

        private void passbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            var myDash = new SignUp();
            myDash.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
/* if ((username == "carmilo@gmail.com") && (password == "carmilo"))
               {
                   this.Hide();
                   var myDash = new Dashboard();
                   myDash.Show();
               }
               else
               {
                   MessageBox.Show("Invalid Email or Password", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }*/