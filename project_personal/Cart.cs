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
    public partial class Cart : Form {
        string query = "";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        int total = 0;
        int newQuantity = 0;
        List<int> listPs_id = new List<int>();
        List<int> list_in_stock = new List<int>();
        List<int> list_currQuan = new List<int>();
        List<int> list_unitPrice = new List<int>();
        public Cart() {
            InitializeComponent();
        }

        private void Cart_Load(object sender, EventArgs e) {
            loadCart();
        }

        private void loadCart() {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT p.product_name AS 'Product', s.size_name AS 'SIZE', p.price AS 'Unit Price', c.quantity AS 'Quantity', p.price * c.quantity AS 'Subtotal', c.ps_id, sp.in_stock " +
                "FROM cart_item AS c " +
                "JOIN size_product AS sp " +
                "ON c.ps_id = sp.ps_id " +
                "JOIN size AS s " +
                "ON sp.s_id = s.s_id " +
                "JOIN product AS p " +
                "ON sp.p_id = p.product_id " +
                $"WHERE c.u_id = {GlobalVar.user_id};";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add("product_name", "Product");
            dgvCart.Columns.Add("size_name", "Size");
            dgvCart.Columns.Add("unit_price", "Unit Prize");
            dgvCart.Columns.Add("quantity", "Quantity");
            dgvCart.Columns.Add("subtotal", "Subtotal");
            listPs_id.Clear();
            list_in_stock.Clear();
            list_currQuan.Clear();
            list_unitPrice.Clear();
            dgvCart.Rows.Clear();
            total = 0;
            newQuantity = 0;
            btnUpdateItemQuantity.Enabled = false;
            btnDeleteItem.Enabled = false;
            txtNewQuantity.Visible = false;
            btnNewQuantityAdd1.Visible = false;
            btnNewQuantitySub1.Visible = false;
            while (reader.Read()) {
                listPs_id.Add((int)reader["ps_id"]);
                list_in_stock.Add((int)reader["in_stock"]);
                list_currQuan.Add((int)reader["Quantity"]);
                list_unitPrice.Add((int)reader["Unit Price"]);
                dgvCart.Rows.Add((string)reader["Product"], (string)reader["Size"], ((int)reader["Unit Price"]).ToString(), ((int)reader["Quantity"]).ToString(), ((int)reader["Subtotal"]).ToString());
                total += (int)reader["Subtotal"];
            }
            if (dgvCart.Rows.Count == 2) {
                lblItemsCount.Text = "1 item";
            }
            else if (dgvCart.Rows.Count > 2) {
                lblItemsCount.Text = $"{dgvCart.Rows.Count - 1} items";
            }
            else {
                lblItemsCount.Text = "0 item";
            }
            lblTotal.Text = $"${total}";
            reader.Close();
            con.Close();
        }

        private void btnDeleteItem_Click(object sender, EventArgs e) {
            if (dgvCart.SelectedRows.Count > 0 && dgvCart.Rows.Count > 1 && dgvCart.SelectedRows[0].Index < dgvCart.Rows.Count - 1) {
                if (MessageBox.Show("Are you sure you want to delete the selected item in the cart?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    int selected_ps_id = listPs_id[dgvCart.SelectedRows[0].Index];
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "DELETE FROM cart_item " +
                            $"WHERE ps_id = {selected_ps_id} AND u_id = {GlobalVar.user_id};";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    loadCart();
                }
            }
        }

        private void deleteAllItemHelper() {
            if (dgvCart.SelectedRows.Count > 0 && dgvCart.Rows.Count > 1) {
                if (MessageBox.Show("Are you sure you want to delete all items in the cart?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    int selected_ps_id = listPs_id[dgvCart.SelectedRows[0].Index];
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "DELETE FROM cart_item " +
                            $"WHERE u_id = {GlobalVar.user_id};";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    loadCart();
                }
            }
        }
        private void btnDeleteAll_Click(object sender, EventArgs e) {
            deleteAllItemHelper();
        }

        private void dgvCart_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (dgvCart.SelectedRows.Count > 0 && dgvCart.Rows.Count > 1 && dgvCart.SelectedRows[0].Index < dgvCart.Rows.Count - 1) {
                btnUpdateItemQuantity.Enabled = true;
                btnDeleteItem.Enabled = true;
                txtNewQuantity.Visible = true;
                btnNewQuantityAdd1.Visible = true;
                btnNewQuantitySub1.Visible = true;
                newQuantity = list_currQuan[dgvCart.SelectedRows[0].Index];
                txtNewQuantity.Text = newQuantity.ToString();
            }
        }

        private void txtNewQuantity_TextChanged(object sender, EventArgs e) {
            int quantityTemp = newQuantity;
            if (!Int32.TryParse(txtNewQuantity.Text, out newQuantity)) {
                MessageBox.Show("Invalid quantity", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewQuantity.Text = list_currQuan[dgvCart.SelectedRows[0].Index].ToString();
            }
            else if (newQuantity > list_in_stock[dgvCart.SelectedRows[0].Index]) {
                MessageBox.Show($"The input quantity is greater than the in stock quantity({list_in_stock[dgvCart.SelectedRows[0].Index]}).", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                newQuantity = quantityTemp;
                txtNewQuantity.Text = newQuantity.ToString();
            }
        }

        private void btnNewQuantityAdd1_Click(object sender, EventArgs e) {
            // since the newQuantity and the txtNewQuantity are always valid number, we don't have to check it here
            if (newQuantity < list_in_stock[dgvCart.SelectedRows[0].Index]) {
                newQuantity++;
                txtNewQuantity.Text = newQuantity.ToString();
            }
            else {
                MessageBox.Show("The quantity in the input box is the upper bound of current in stock.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNewQuantitySub1_Click(object sender, EventArgs e) {
            if (newQuantity > 1) {
                newQuantity--;
                txtNewQuantity.Text = newQuantity.ToString();
            }
        }

        private void btnUpdateItemQuantity_Click(object sender, EventArgs e) {
            if (dgvCart.SelectedRows.Count > 0 && dgvCart.Rows.Count > 1 && dgvCart.SelectedRows[0].Index < dgvCart.Rows.Count - 1) {
                if (newQuantity > 0) {
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "UPDATE cart_item " +
                            $"SET quantity = {newQuantity} " +
                            $"WHERE u_id = {GlobalVar.user_id} AND ps_id = {listPs_id[dgvCart.SelectedRows[0].Index]}";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    loadCart();
                }
                else {
                    MessageBox.Show("Can not update quantity to 0", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    newQuantity = list_currQuan[dgvCart.SelectedRows[0].Index];
                    txtNewQuantity.Text = newQuantity.ToString();
                }
            }
            else {
                MessageBox.Show("No item is selected", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e) {
            if (dgvCart.Rows.Count > 1 && MessageBox.Show("Are you sure you want to checkout all the items in the cart?", "Confirm CheckOut", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                // insert into order_item table
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "INSERT INTO order_item (ps_id, quantity, unitprice) VALUES "; // order_id is null for now
                for (int i = 0; i < listPs_id.Count; i++) {
                    query += $"({listPs_id[i]}, {list_currQuan[i]}, {list_unitPrice[i]})";
                    if (i < listPs_id.Count - 1) {
                        query += ", ";
                    }
                    else {
                        query += ";";
                    }
                }
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();

                // insert into orders table
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "INSERT INTO orders (buyer_id, order_time, total_price, status) VALUES " +
                        $"({GlobalVar.user_id}, @time, {total}, 1);";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();

                // find the latest order_id 
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "SELECT MAX(order_id) " +
                        "FROM orders;";
                cmd = new SqlCommand(query, con);
                reader = cmd.ExecuteReader();
                reader.Read();
                int currOrderId = (int)reader[0];
                reader.Close();
                con.Close();

                // set the order_id to the order_item whose order_id is null
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "UPDATE order_item " +
                        $"SET order_id = {currOrderId} " +
                        "WHERE order_id IS NULL;";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Order placed successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                deleteAllItemHelper();
            }
        }
    }
}
