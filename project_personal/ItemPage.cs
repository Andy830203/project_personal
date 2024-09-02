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
    public partial class ItemPage : Form {
        public int product_id = 0;
        string query = "";
        List<int> list_ps_id = new List<int>();
        List<int> size_in_stock = new List<int>();
        int inStock = 0;
        int unit_price = 0;
        int quantity = 0;
        int imageListIndex = 0;
        public ItemPage() {
            InitializeComponent();
        }

        private void ItemPage_Load(object sender, EventArgs e) {
            SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT sp.ps_id, sp.s_id, s.size_name, sp.in_stock, p.product_name, p.description, p.price " +
                    "FROM size_product AS sp " +
                    "JOIN size AS s " +
                    "ON sp.s_id = s.s_id " +
                    "JOIN product AS p " +
                    "ON sp.p_id = p.product_id " +
                    $"WHERE sp.p_id = {product_id};";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            bool info_loaded = false;
            cBoxSize.Items.Clear();
            while (reader.Read()) {
                list_ps_id.Add((int)reader["ps_id"]);
                // sizeList.Add((string)reader["size_name"]);
                size_in_stock.Add((int)reader["in_stock"]);
                cBoxSize.Items.Add((string)reader["size_name"]);
                // sizeIndexInDB.Add((int)reader["s_id"]);
                if (!info_loaded) {
                    lblItemName.Text = (string)reader["product_name"];
                    lblDescription.Text = (string)reader["description"];
                    unit_price = (int)reader["price"];
                    lblUPrice.Text = (unit_price).ToString();
                    info_loaded = true;
                }
            }
            reader.Close();
            con.Close();
            // load images
            imageListCurrItem.Images.Clear();
            imageListCurrItem.ImageSize = new Size(200, 200);
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT i.image_name, c.category_name " +
                    "FROM product_image AS i " +
                    "JOIN product AS p " +
                    "ON i.product_id = p.product_id " +
                    "JOIN category AS c " +
                    "ON p.category = c.category_id " +
                    $"WHERE i.product_id = {product_id};";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            while (reader.Read()) {
                string imagePath = GlobalVar.image_folder + @"\" + (string)reader["category_name"] + @"\" + (string)reader["image_name"];
                System.IO.FileStream fs = System.IO.File.OpenRead(imagePath);
                Image currImage = Image.FromStream(fs);
                imageListCurrItem.Images.Add(currImage);
                fs.Close();
            }
            reader.Close();
            con.Close();
            pBoxItem.Image = imageListCurrItem.Images[0];
            imageListIndex = 0;
        }

        private void cBoxSize_SelectedIndexChanged(object sender, EventArgs e) {
            lbllInStock.Text = size_in_stock[cBoxSize.SelectedIndex].ToString();
            inStock = size_in_stock[cBoxSize.SelectedIndex];
            quantity = 0;
            txtQuantity.Text = "0";
        }
        // cbox.selectedIndex == 5
        private void btnQuantityAdd1_Click(object sender, EventArgs e) {
            if (quantity < inStock) {
                quantity++;
                txtQuantity.Text = quantity.ToString();
            }
        }

        private void btnQuantitySub1_Click(object sender, EventArgs e) {
            if (quantity > 0) {
                quantity--;
                txtQuantity.Text = quantity.ToString();
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e) {
            int newQuantity = 0;
            if (!Int32.TryParse(txtQuantity.Text, out newQuantity)) {
                MessageBox.Show("Invalid quantity", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (newQuantity > inStock) {
                MessageBox.Show("The input quantity is greater than the in stock quantity.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                quantity = newQuantity;
                lblTPrice.Text = (unit_price * quantity).ToString();
            }
            txtQuantity.Text = quantity.ToString();
        }

        private void BtnBack_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnNextImage_Click(object sender, EventArgs e) {
            if (imageListIndex < imageListCurrItem.Images.Count - 1) {
                imageListIndex++;
            }
            else {
                imageListIndex = 0;
            }
            pBoxItem.Image = imageListCurrItem.Images[imageListIndex];
        }

        private void btnPrevImage_Click(object sender, EventArgs e) {
            if (imageListIndex > 0) {
                imageListIndex--;
            }
            else {
                imageListIndex = imageListCurrItem.Images.Count - 1;
            }
            pBoxItem.Image = imageListCurrItem.Images[imageListIndex];
        }
        private void BtnAddToCart_Click(object sender, EventArgs e) {
            int currPs_idInCartQuantity = -1;
            SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            bool validOrder = true;
            if (cBoxSize.SelectedIndex < 0) {
                MessageBox.Show("Please select size", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                validOrder = false;
            }
            if (validOrder && txtQuantity.Text == "0") {
                MessageBox.Show("The quantity must be greater than 0", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                validOrder = false;
            }
            if (validOrder) {
                query = "SELECT * " +
                    "FROM cart_item " +
                    $"WHERE ps_id = {list_ps_id[cBoxSize.SelectedIndex]} AND u_id = {GlobalVar.user_id};";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    // this user already has this item in his/her cart 
                    currPs_idInCartQuantity = (int)reader["quantity"];
                }
                reader.Close();
                con.Close();

                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                if (currPs_idInCartQuantity > 0) {
                    query = "UPDATE cart_item " +
                            $"SET quantity = {Math.Min(inStock, currPs_idInCartQuantity + Convert.ToInt32(txtQuantity.Text))} " +
                            $"WHERE ps_id = {list_ps_id[cBoxSize.SelectedIndex]} AND u_id = {GlobalVar.user_id};";
                }
                else {
                    query = "INSERT INTO cart_item (ps_id, u_id, quantity) " +
                            $"VALUES ({list_ps_id[cBoxSize.SelectedIndex]}, {GlobalVar.user_id}, {Math.Min(inStock, Convert.ToInt32(txtQuantity.Text))});";
                }
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("This item is added into the cart successfully.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }  
        }
    }
}
