using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;

namespace project_personal {
    public partial class FormSignUp : Form {
        SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
        public FormSignUp() {
            InitializeComponent();
        }

        private void FormSignUp_Load(object sender, EventArgs e) {
            scsb.DataSource = @".";
            scsb.InitialCatalog = "ppdb";
            scsb.IntegratedSecurity = true;
            GlobalVar.strDBConnectionString = scsb.ConnectionString.ToString();
            dtpBirthday.Value = new DateTime(1901, 1, 1, 5, 5, 5); // default birthday
        }

        private void BtnBackSignIn_Click(object sender, EventArgs e) {
            GlobalVar.NowSignIn = true;
            this.Close();
        }

        private void BtnCreateAcc_Click(object sender, EventArgs e) {
            // check the username, password and other info are valid;
            bool valid = true;
            // username
            if (txtUsername.Text.Length == 0) {
                MessageBox.Show("The username is required", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                valid = false;
            }
            else if (txtUsername.Text.Length <= 5) {
                MessageBox.Show("The username must be 6 characters or more", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                valid = false;
            }
            else {
                foreach (char c in txtUsername.Text) {
                    if (!((c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122))) {
                        MessageBox.Show("The username must contain only uppercase letters, lowercase letters and numbers", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        valid = false;
                        break;
                    }
                }
            }
            // password
            if (valid) {
                if (txtPassword.Text.Length == 0) {
                    MessageBox.Show("The password is required", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;

                }
                else if (txtPassword.Text.Length <= 5) {
                    MessageBox.Show("The password must be 6 characters or more", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;
                }
                else {
                    foreach (char c in txtPassword.Text) {
                        if (!((c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122))) {
                            MessageBox.Show("The password must contain only uppercase letters, lowercase letters and numbers", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            valid = false;
                            break;
                        }
                    }
                }
            }
            // phone
            if (valid) {
                if (txtPhone.Text.Length < 10 && txtPhone.Text.Length > 0) {
                    MessageBox.Show("The phone number must contain 10 Arabic numerals", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;
                }
                if (valid) {
                    foreach (char c in txtPhone.Text) {
                        if (!(c >= 48 && c <= 57)) {
                            MessageBox.Show("The phone number must contain only numbers", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            valid = false;
                            break;
                        }
                    }
                }
            }
            // email
            if (valid && txtEmail.Text.Length > 0) {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // regular expressions
                if (!Regex.IsMatch(txtEmail.Text, pattern, RegexOptions.IgnoreCase)) {
                    MessageBox.Show("The input Email must be in valid email format", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;
                }
            }
            // check if the birthday is the default value
            bool birthdaySet = false;
            DateTime defaultValue = new DateTime(1901, 1, 1, 5, 5, 5);
            if (dtpBirthday.Value != defaultValue) {
                birthdaySet = true;
            }
            // check there isn't an identical username in the database,
            if (valid) {
                SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                string query = "SELECT * FROM member WHERE username = @newUserName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@newUsername", txtUsername.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    MessageBox.Show("This username has already been registered", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    reader.Close();
                    con.Close();
                }
                else {
                    try {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        if (birthdaySet) {
                            query = "INSERT INTO member (username, password, real_name, phone, email, address, birthday) VALUES (@newUserName, @newPassword, @newName, @newPhone, @newEmail, @newAddress, @newBirthday);";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@newUsername", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@newPassword", txtPassword.Text);
                            cmd.Parameters.AddWithValue("@newName", txtName.Text);
                            cmd.Parameters.AddWithValue("@newPhone", txtPhone.Text);
                            cmd.Parameters.AddWithValue("@newEmail", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@newAddress", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@newBirthday", dtpBirthday.Value.Date);
                        }
                        else {
                            query = "INSERT INTO member (username, password, real_name, phone, email, address) VALUES (@newUserName, @newPassword, @newName, @newPhone, @newEmail, @newAddress);";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@newUsername", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@newPassword", txtPassword.Text);
                            cmd.Parameters.AddWithValue("@newName", txtName.Text);
                            cmd.Parameters.AddWithValue("@newPhone", txtPhone.Text);
                            cmd.Parameters.AddWithValue("@newEmail", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@newAddress", txtAddress.Text);
                        }
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("A new account is created successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVar.NowSignIn = true;
                        this.Close();
                    }
                    catch (Exception error) {
                        MessageBox.Show("Register failed", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
