using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace project_personal {
    public partial class FormSignIn : Form {
        Dictionary<string, string> user_password_pair = new Dictionary<string, string>();
        

        public FormSignIn() {
            InitializeComponent();
        }

        private void FormSignIn_Load(object sender, EventArgs e) {
            GlobalVar.scsb = new SqlConnectionStringBuilder();
            GlobalVar.scsb.DataSource = @".";
            GlobalVar.scsb.InitialCatalog = "ppdb";
            GlobalVar.scsb.IntegratedSecurity = true;
            GlobalVar.strDBConnectionString = GlobalVar.scsb.ConnectionString.ToString();
            LoadPairs();
        }

        private void LoadPairs() { 
            // load username and password pairs into dictionary
            user_password_pair.Clear();
            SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            string query = "SELECT * FROM member;";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                user_password_pair.Add(reader["username"].ToString(), reader["password"].ToString());
            }
            reader.Close();
            con.Close();
        }
        private void BtnVisibleSwitch_Click(object sender, EventArgs e) {
            if (txtPassword.UseSystemPasswordChar) {
                txtPassword.UseSystemPasswordChar = false;
                BtnVisibleSwitch.Image = Image.FromFile(@"C:\iSpan\project_personal\images\icons\invisible.png");
            }
            else {
                txtPassword.UseSystemPasswordChar = true;
                BtnVisibleSwitch.Image = Image.FromFile(@"C:\iSpan\project_personal\images\icons\icons8-show-password-24.png");
            }
        }

        private void BtnLogIn_Click(object sender, EventArgs e) {
            // check whether the input username and password is in the dictionary
            if (user_password_pair.ContainsKey(txtUsername.Text) && user_password_pair[txtUsername.Text] == txtPassword.Text) {
                SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                string query = "SELECT * FROM member WHERE username = @username;";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                // set current user info into global variables
                GlobalVar.user_id = Convert.ToInt32(reader["u_id"]);
                GlobalVar.user_name = reader["username"].ToString();
                GlobalVar.user_level = Convert.ToInt32(reader["userlevel"]);
                GlobalVar.user_status = Convert.ToInt32(reader["userstatus"]);
                GlobalVar.user_realName = reader["real_name"].ToString();
                GlobalVar.user_phone = reader["phone"].ToString();
                GlobalVar.user_email = reader["email"].ToString();
                GlobalVar.user_address = reader["address"].ToString();
                if (reader["birthday"] != DBNull.Value) {
                    GlobalVar.user_birthday = Convert.ToDateTime(reader["birthday"]);
                }
                else {
                    GlobalVar.hasBirthday = false;
                }
                reader.Close();
                con.Close();
                GlobalVar.NowSignIn = false;
                this.Visible = false;
                FormMain formMain = new FormMain();
                formMain.ShowDialog();
                if (!GlobalVar.NowSignIn) {
                    this.Close();
                }
                else {
                    txtUsername.Clear();
                    txtPassword.Clear();
                    this.Visible = true;
                }
            }
            else {
                MessageBox.Show("Invalid username or password", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void BtnSignUp_Click(object sender, EventArgs e) {
            GlobalVar.NowSignIn = false;
            FormSignUp newSignup = new FormSignUp();
            this.Visible = false;
            newSignup.ShowDialog();
            // If X button is pressed, turn off the entire program
            if (!GlobalVar.NowSignIn) {
                this.Close();
            }
            // If sign up successfully or the user back to login page, back to login form
            else {
                txtUsername.Clear();
                txtPassword.Clear();
                this.Visible = true;
                LoadPairs();
            }
        }
        // visit as a guest
        private void btnCAG_Click(object sender, EventArgs e) {
            GlobalVar.NowSignIn = false;
            this.Visible = false;
            FormMain formMain = new FormMain();
            formMain.ShowDialog();
            if (!GlobalVar.NowSignIn) {
                this.Close();
            }
            else {
                txtUsername.Clear();
                txtPassword.Clear();
                this.Visible = true;
                LoadPairs();
            }
        }
    }
}
