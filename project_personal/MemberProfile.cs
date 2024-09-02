using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_personal {
    public partial class MemberProfile : Form {
        string query = "";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        int id_for_retrieved = 0;
        public MemberProfile() {
            InitializeComponent();
        }

        private void MemberProfile_Load(object sender, EventArgs e) {
            if (!(GlobalVar.user_level >= 1 && GlobalVar.user_level <= 3)) { // guest
                gBoxSearch.Visible = false;
                this.Width -= 300;
            }
            if (GlobalVar.user_level == 4) {
                cBoxUserLevel.Enabled = false;
            }
            cBoxUserStatusSearch.SelectedIndex = 0;
            cBoxUserLevelSearch.SelectedIndex = 0;
            ProfileLoad(GlobalVar.user_id);
            dgvSearchResult.Columns.Clear();
            dgvSearchResult.Columns.Add("id", "ID");
            dgvSearchResult.Columns.Add("username", "Usernname");
        }
        private void ProfileLoad(int id) {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT m.u_id, m.username, m.password, m.real_name, m.phone, m.email, m.address, m.birthday, m.userstatus, m.userlevel " +
                    "FROM member AS m " +
                    "JOIN user_level AS ul " +
                    "ON m.userlevel = ul.level_id " +
                    "JOIN user_status AS us " +
                    "ON m.userstatus = us.status_id " +
                    $"WHERE m.u_id = {id};";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            reader.Read();
            lblID.Text = $"{(int)reader["u_id"]}";
            lblUsername.Text = (string)reader["username"];
            txtPassword.Text = (string)reader["password"];
            txtName.Text = (reader["real_name"] != DBNull.Value ? (string)reader["real_name"]: "");
            txtPhone.Text = (reader["phone"] != DBNull.Value ? (string)reader["phone"] : "");
            txtEmail.Text = (reader["email"] != DBNull.Value ? (string)reader["email"] : "");
            txtAddress.Text = (reader["address"] != DBNull.Value ? (string)reader["address"] : "");
            cBoxUserSatatus.SelectedIndex = (int)reader["userstatus"] - 1;
            cBoxUserLevel.SelectedIndex = (int)reader["userlevel"] - 1;
            checkBoxEdit.Checked = false;
            if (reader["birthday"] == DBNull.Value) {
                dtpBirthday.Enabled = false;
            }
            else {
                dtpBirthday.Value = reader.GetDateTime(7);
            }
            if ((int)reader["userlevel"] < GlobalVar.user_level || ((int)reader["userlevel"] == GlobalVar.user_level && id != GlobalVar.user_id)) {
                cBoxUserLevel.Enabled = false;
                cBoxUserSatatus.Enabled = false;
            }
            if (id != GlobalVar.user_id) {
                txtPassword.Enabled = false;
                txtName.Enabled = false;
                txtPhone.Enabled = false;
                txtEmail.Enabled = false;
                txtAddress.Enabled = false;
            }
            else {
                txtPassword.Enabled = true;
                txtName.Enabled = true;
                txtPhone.Enabled = true;
                txtEmail.Enabled = true;
                txtAddress.Enabled = true;
            }
            reader.Close();
            con.Close();
            id_for_retrieved = id;
            if (id != GlobalVar.user_id) {
                BtnVisibleSwitch.Visible = false;
            }
            else {
                BtnVisibleSwitch.Visible = true;
            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e) {
            if (id_for_retrieved != 0) {
                ProfileLoad(id_for_retrieved);
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e) {
            bool valid = true;
            // password check
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
            // phone number chaeck
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
            // email check
            if (valid && txtEmail.Text.Length > 0) {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // regular expressions
                if (!Regex.IsMatch(txtEmail.Text, pattern, RegexOptions.IgnoreCase)) {
                    MessageBox.Show("The input Email must be in valid email format", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;
                }
            }
            // user level
            if (valid) {
                if (cBoxUserLevel.SelectedIndex + 1 < GlobalVar.user_level) {
                    MessageBox.Show("The set user level can not be higher or equal to current user level", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    valid = false;
                }
            }
            if (valid) {
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "UPDATE member " +
                        $"SET password = @pw , " +
                        $"real_name = @rname , " +
                        $"phone = @phone , " +
                        $"email = @email , " +
                        $"address = @address , ";
                if (checkBoxEdit.Checked) {
                    query += $"birthday = @bd , ";
                }
                query += $"userlevel = {cBoxUserLevel.SelectedIndex + 1}, " +
                         $"userstatus = {cBoxUserSatatus.SelectedIndex + 1} " +
                         $"WHERE u_id = {id_for_retrieved};";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pw", txtPassword.Text);
                cmd.Parameters.AddWithValue("@rname", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@bd", dtpBirthday.Value.Date);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("updated successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkBoxEdit_CheckedChanged(object sender, EventArgs e) {
            if (!checkBoxEdit.Checked) {
                dtpBirthday.Enabled = false;
            }
            else {
                dtpBirthday.Enabled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM member AS m " +
                    "WHERE 1 = 1";
            if (txtIDSearch.Text.Length > 0) {
                query += "AND u_id LIKE @id ";
            }
            if (txtUsernameSearch.Text.Length > 0) {
                query += "AND username LIKE @username ";
            }
            if (cBoxUserStatusSearch.SelectedIndex > 0) {
                query += $"AND userstatus = {cBoxUserStatusSearch.SelectedIndex} ";
            }
            if (cBoxUserLevelSearch.SelectedIndex > 0) {
                query += $"AND userlevel = {cBoxUserLevelSearch.SelectedIndex} ";
            }
            if (txtNameSearch.Text.Length > 0) {
                query += "AND real_name LIKE @realName ";
            }
            if (txtPhoneSearch.Text.Length > 0) {
                query += "AND phone LIKE @phone ";
            }
            if (txtEmailSearch.Text.Length > 0) {
                query += "AND email LIKE @email ";
            }
            if (txtAddressSearch.Text.Length > 0) {
                query += "AND address LIKE @address";
            }
            query += ";";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", "%" + txtIDSearch.Text + "%");
            cmd.Parameters.AddWithValue("@username", "%" + txtUsernameSearch.Text + "%");
            cmd.Parameters.AddWithValue("@realname", "%" + txtNameSearch.Text + "%");
            cmd.Parameters.AddWithValue("@phone", "%" + txtPhoneSearch.Text + "%");
            cmd.Parameters.AddWithValue("@email", "%" + txtEmailSearch.Text + "%");
            cmd.Parameters.AddWithValue("@address", "%" + txtAddressSearch.Text + "%");
            reader = cmd.ExecuteReader();
            dgvSearchResult.Rows.Clear();
            while (reader.Read()) {
                dgvSearchResult.Rows.Add((int)reader["u_id"], (string)reader["username"]);
            }
            reader.Close();
            con.Close();
        }

        private void dgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (dgvSearchResult.SelectedRows.Count > 0 && dgvSearchResult.Rows.Count > 1 && dgvSearchResult.SelectedRows[0].Index < dgvSearchResult.Rows.Count - 1) {
                ProfileLoad((int)dgvSearchResult.SelectedRows[0].Cells["id"].Value);
            }
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
    }
}
