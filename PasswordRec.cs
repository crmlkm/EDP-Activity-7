using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace Act4_EDP
{
    public partial class PasswordRec : Form
    {
        //string randomCode;
        //public static string to;
        public PasswordRec()
        {
            InitializeComponent();
        }

        private void sendMailButton_Click(object sender, EventArgs e)
        {
            string rec_email = this.textBox1.Text;

            try
            {
                MySqlConnection conn;
                string myConnectionString;
                myConnectionString = "server=localhost;uid=root;" +
                    "pwd=carmilo;database=clinic_db";

                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                string sql = "SELECT COUNT(*) FROM useraccounts WHERE rec_email = @myrecemail";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@myrecemail", rec_email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    // If the email exists, prompt the user to reset the password
                    DialogResult result = MessageBox.Show("Do you want to reset your password?", "Reset Password", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        // If the user chooses Yes, delete the password from the database
                        string deleteSql = "UPDATE useraccounts SET Password = NULL WHERE rec_email = @myrecemail";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn);
                        deleteCmd.Parameters.AddWithValue("@myrecemail", rec_email);
                        deleteCmd.ExecuteNonQuery();

                        // Prompt the user to input a new password
                        string newPassword = Prompt.ShowDialog("Enter your new password: ", "New Password");

                        // You can add validation here to ensure newPassword is not empty

                        // Update the database with the new password
                        string updateSql = "UPDATE useraccounts SET Password = @newPassword WHERE rec_email = @myrecemail";
                        MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                        updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@myrecemail", rec_email);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Password updated successfully!");

                        this.Close();

                        Login gotolog = new Login();
                        gotolog.ShowDialog();

                      
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email.");
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
                        MessageBox.Show("Invalid Recovery Email, Please Try Again");
                        break;
                    // Add more cases if needed
                    default:
                        MessageBox.Show($"An error occurred: {ex.Message}");
                        break;
                }
            }

        }

    

public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 400;
            prompt.Height = 150;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen; // Center the form

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300 };

            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.ShowDialog();

            return textBox.Text;
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var goback = new Login();
            goback.Show();
            this.Hide();
        }
    }//end

}

/*
 string from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            to = (textBox1.Text).ToString();
            from = "carmilo@gmail.com";
            pass = "carmilo";
            messageBody = "Your Reset Code is " + randomCode;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Password Resetting Code";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);
                MessageBox.Show("Code Sent Successfully");
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


 if (randomCode == (textBox2.Text).ToString())
            {
                to = textBox1.Text;
                ResetPass rp = new ResetPass();
                this.Hide();
                rp.Show();
            }
            else
            {
                MessageBox.Show("Wrong Code");
            }
 */