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

namespace project_personal {
    public partial class OrdersReview : Form {
        string query = "";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public OrdersReview() {
            InitializeComponent();
        }

        private void OrdersReview_Load(object sender, EventArgs e) {
            dgvOrders.Columns.Clear();
            dgvOrders.Columns.Add("order_id", "Order ID");
            dgvOrders.Columns.Add("buyer_id", "Buyer ID");
            dgvOrders.Columns.Add("item_quantity", "Item Quantity");
            dgvOrders.Columns.Add("total_price", "Total Price");
            dgvOrders.Columns.Add("order_time", "Order Time");
            dgvOrders.Columns.Add("shipped_time", "Shipped Time");
            dgvOrders.Columns.Add("order_status", "Status");
            dgvOrderDetails.Columns.Clear();
            dgvOrderDetails.Columns.Add("p_name", "Product Name");
            dgvOrderDetails.Columns.Add("category", "Category");
            dgvOrderDetails.Columns.Add("size", "Size");
            dgvOrderDetails.Columns.Add("quantity", "Quantity");
            dgvOrderDetails.Columns.Add("subtotal", "Subtotal");
            dgvOrderDetails.Columns["size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderDetails.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderDetails.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // status combobox load
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM order_status;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            while (reader.Read()) {
                CBCurrentStatus.Items.Add((string)reader["status_name"]);
                CBchangedStatus.Items.Add((string)reader["status_name"]);
            }
            CBchangedStatus.SelectedIndex = 0;
            reader.Close();
            con.Close();

        }

        private void CBCurrentStatus_SelectedIndexChanged(object sender, EventArgs e) {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT o.order_id, o.buyer_id, temp.itemQuan, o.total_price, o.order_time, o.shipped_time, os.status_name, o.status " +
                    "FROM orders AS o " +
                    "JOIN order_status AS os " +
                    "ON o.status = os.status_id " +
                    "JOIN (SELECT order_id, COUNT(*) AS itemQuan " +
                          "FROM order_item " +
                          "GROUP BY order_id) AS temp " +
                    "ON o.order_id = temp.order_id " +
                    $"WHERE o.status = {CBCurrentStatus.SelectedIndex + 1}";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            dgvOrders.Rows.Clear();
            while (reader.Read()) {
                dgvOrders.Rows.Add((int)reader[0], (int)reader[1], (int)reader[2], (int)reader[3], (DateTime)reader[4], reader[5] != DBNull.Value ? (DateTime)reader[5] : reader[5], (string)reader[6]);
            }
            reader.Close();
            con.Close();
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e) {
            dgvOrderDetails.Rows.Clear();
            if (dgvOrders.SelectedRows.Count > 0 && dgvOrders.Rows.Count > 1 && dgvOrders.SelectedRows[0].Index < dgvOrders.Rows.Count - 1) {
                int orderId = (int)dgvOrders.SelectedRows[0].Cells["order_id"].Value;
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "SELECT oi.order_id, p.product_name, s.size_name, c.category_name ,oi.quantity, oi.quantity * oi.unitprice AS subtotal " +
                        "FROM order_item AS oi " +
                        "JOIN size_product AS sp " +
                        "ON oi.ps_id = sp.ps_id " +
                        "JOIN product AS p " +
                        "ON sp.p_id = p.product_id " +
                        "JOIN size AS s " +
                        "ON sp.s_id = s.s_id " +
                        "JOIN orders AS o " +
                        "ON oi.order_id = o.order_id " +
                        "JOIN category AS c " +
                        "ON p.category = c.category_id " +
                        $"WHERE oi.order_id = {orderId}";
                cmd = new SqlCommand(query, con);
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    dgvOrderDetails.Rows.Add((string)reader[1], (string)reader[3], (string)reader[2], (int)reader[4], (int)reader[5]);
                }
                reader.Close() ; 
                con.Close();
            }
        }

        private void btnSaveStatusChanged_Click(object sender, EventArgs e) {
            if (dgvOrders.SelectedRows.Count > 0 && dgvOrders.Rows.Count > 1 && dgvOrders.SelectedRows[0].Index < dgvOrders.Rows.Count - 1) {
                int orderId = (int)dgvOrders.SelectedRows[0].Cells["order_id"].Value;
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "UPDATE orders " +
                        $"SET status = {CBchangedStatus.SelectedIndex + 1} " +
                        $"WHERE order_id = {orderId};";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                if (CBchangedStatus.SelectedIndex == 2) {
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "UPDATE orders " +
                            $"SET shipped_time = @time " +
                            $"WHERE order_id = {orderId};";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("The status is changed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show("No order is selected.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dgvOrders.Rows.Clear();
            dgvOrderDetails.Rows.Clear();
            CBCurrentStatus.SelectedIndex = 0;
        }
    }
}
