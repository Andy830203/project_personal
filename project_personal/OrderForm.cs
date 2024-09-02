using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;

namespace project_personal {
    public partial class OrderForm : Form {
        string query = "";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        List<ArrayList> listOrders = new List<ArrayList>(); // arraylist: order_id, buyer_id, itemQuan, total_price, order_time, shipped_time
        List<ArrayList> listOrderItems = new List<ArrayList>(); // arraylist: order_id, product_name, category_name, size_name, quantity, subtotal
        IEnumerable matchedOrders = null;
        IEnumerable matchedOrderItems = null;
        int currCheckedId = 0;
        public OrderForm() {
            InitializeComponent();
        }

        private void OrderForm_Load(object sender, EventArgs e) {
            // load all orders data into list
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT o.order_id, o.buyer_id, temp.itemQuan, o.total_price, o.order_time, o.shipped_time, os.status_name, o.status " +
                    "FROM orders AS o " +
                    "JOIN order_status AS os " +
                    "ON o.status = os.status_id " +
                    "JOIN (SELECT order_id, COUNT(*) AS itemQuan " +
                          "FROM order_item " +
                          "GROUP BY order_id) AS temp " +
                    "ON o.order_id = temp.order_id;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            listOrders.Clear();
            while (reader.Read()) {
                ArrayList order = new ArrayList();
                order.Add((int)reader["order_id"]);
                order.Add((int)reader["buyer_id"]);
                order.Add((int)reader["itemQuan"]);
                order.Add((int)reader["total_price"]);
                order.Add(reader.GetDateTime(4));
                if (reader["shipped_time"] != DBNull.Value) {
                    order.Add(reader.GetDateTime(5));
                }
                else {
                    order.Add(DBNull.Value);
                }
                order.Add((string)reader["status_name"]);
                order.Add((int)reader["status"]);
                listOrders.Add(order);
            }
            reader.Close();
            con.Close();

            // Load all order items into List of Arraylist
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
                    "ON p.category = c.category_id;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            listOrderItems.Clear();
            while (reader.Read()) {
                ArrayList orderItem = new ArrayList();
                orderItem.Add((int)reader["order_id"]);
                orderItem.Add((string)reader["product_name"]);
                orderItem.Add((string)reader["category_name"]);
                orderItem.Add((string)reader["size_name"]);
                orderItem.Add((int)reader["quantity"]);
                orderItem.Add((int)reader["subtotal"]);
                listOrderItems.Add(orderItem);
            }
            reader.Close();
            con.Close();

            //dgv columns setup
            lblOrderItemsCount.Visible = false;
            dgvOrders.Columns.Clear();
            dgvOrders.Columns.Add("order_id", "Order ID");
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
            cBoxOrderBy.SelectedIndex = 0;
            currCheckedId = GlobalVar.user_id;
            OrdersLoad(currCheckedId, 0, 0); // default to load data of current user
            txtUserID.Text = GlobalVar.user_id.ToString();
            if (!(GlobalVar.user_level >= 1 && GlobalVar.user_level <= 3)) {
                txtUserID.ReadOnly = true;
                btnSearchID.Visible = false;
            }

            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM order_status;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            cBoxStatusFilter.Items.Add("All");
            while (reader.Read()) {
                cBoxStatusFilter.Items.Add(reader["status_name"]);
            }
            cBoxStatusFilter.SelectedIndex = 0;
            reader.Close();
            con.Close();
        }
        private void OrdersLoad(int userId, int orderByCBoxIndex, int statusCBoxIndex) {
            if (statusCBoxIndex != 0) {
                matchedOrders = listOrders.Where(o => (int)o[1] == userId && (int)o[7] == statusCBoxIndex);
            }
            else {
                matchedOrders = listOrders.Where(o => (int)o[1] == userId);
            }
            List<ArrayList> temp = new List<ArrayList>();
            foreach (ArrayList order in matchedOrders) {
                temp.Add(order);
            }
            switch (orderByCBoxIndex) {
                case 0:
                    matchedOrders = temp.OrderBy(o => o[4]);
                    break;
                case 1:
                    matchedOrders = temp.OrderByDescending(o => o[4]);
                    break;
                case 2:
                    matchedOrders = temp.OrderBy(o => o[3]);
                    break;
                default:
                    matchedOrders = temp.OrderByDescending(o => o[3]);
                    break;
            }
            dgvOrders.Rows.Clear();
            dgvOrderDetails.Rows.Clear();
            foreach (ArrayList order in matchedOrders) {
                dgvOrders.Rows.Add((int)order[0], (int)order[2], (int)order[3], (DateTime)order[4], order[5] != DBNull.Value? (DateTime)order[5]: order[5], (string)order[6]);
            }
            lblOrdersCount.Text = $"{dgvOrders.RowCount - 1} results";
        }


        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (dgvOrders.SelectedRows.Count > 0 && dgvOrders.Rows.Count > 1 && dgvOrders.SelectedRows[0].Index < dgvOrders.Rows.Count - 1) {
                int orderId = (int)dgvOrders.SelectedRows[0].Cells["order_id"].Value;
                matchedOrderItems = listOrderItems.Where(s => (int)s[0] == orderId);
                dgvOrderDetails.Rows.Clear();
                foreach (ArrayList orderItem in matchedOrderItems) {
                    dgvOrderDetails.Rows.Add((string)orderItem[1], (string)orderItem[2], (string)orderItem[3], (int)orderItem[4], (int)orderItem[5]);
                }
                lblOrderItemsCount.Text = $"{dgvOrderDetails.RowCount - 1} results";
                lblOrderItemsCount.Visible = true;
            }
        }

        private void cBoxOrderBy_SelectedIndexChanged(object sender, EventArgs e) {
            OrdersLoad(currCheckedId, cBoxOrderBy.SelectedIndex, cBoxStatusFilter.SelectedIndex);
        }

        private void cBoxStatusFilter_SelectedIndexChanged(object sender, EventArgs e) {
            OrdersLoad(currCheckedId, cBoxOrderBy.SelectedIndex, cBoxStatusFilter.SelectedIndex);
        }

        private void btnSearchID_Click(object sender, EventArgs e) {
            int sUID = GlobalVar.user_id;
            if (!Int32.TryParse(txtUserID.Text, out sUID)) {
                MessageBox.Show("Invalid User ID", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserID.Text = "";
            }
            else {
                cBoxOrderBy.SelectedIndex = 0;
                cBoxStatusFilter.SelectedIndex = 0;
                OrdersLoad(sUID, cBoxOrderBy.SelectedIndex, cBoxStatusFilter.SelectedIndex);
            }
        }
    }
}
