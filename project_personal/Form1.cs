using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_personal {
    public partial class FormMain : Form {
        List<ArrayList> productsAll = new List<ArrayList>(); //arraylist: id, pname, category, brand, price, image_name
        string query = "";
        int pagesAmount = 1;
        int currPage = 1;
        IEnumerable<ArrayList> queryResult = null;
        public FormMain() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            if (GlobalVar.user_level == 0) {
                lblUsername.Text = "Guest";
                btnMyOrders.Visible = false;
                btnProfile.Visible = false;
                btnCart.Visible = false;
                btnSignOut.Text = "Login";
                btnProductsManagement.Visible = false;
                btnOrdersReview.Visible = false;
            }
            else {
                lblUsername.Text = "User: " + GlobalVar.user_name;
                btnSignOut.Text = "Sign Out";
                if (GlobalVar.user_level >= 4) {
                    btnProductsManagement.Visible = false;
                    btnOrdersReview.Visible = false;
                }
            }
            SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            // load all on_shelf products from DB
            query = "SELECT p.*, b.brand_name, c.category_name, i.image_name " +
                    "FROM product AS p " +
                    "JOIN brand AS b " +
                    "ON p.brand = b.brand_id " +
                    "JOIN category AS c " +
                    "ON p.category = c.category_id " +
                    "JOIN (SELECT *, ROW_NUMBER() OVER (PARTITION BY product_id ORDER BY image_id) AS rowNum FROM product_image) AS i " +
                    "ON p.product_id = i.product_id " +
                    "WHERE on_shelf = 1 AND rowNum = 1;";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                ArrayList productInfo = new ArrayList();
                productInfo.Add((int)reader["product_id"]);
                productInfo.Add((string)reader["product_name"]);
                productInfo.Add((string)reader["category_name"]);
                productInfo.Add((string)reader["brand_name"]);
                productInfo.Add((int)reader["price"]);
                productInfo.Add((string)reader["image_name"]);
                productsAll.Add(productInfo);
            }
            reader.Close();
            con.Close();

            // load brand combox items brand
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM brand";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBBrandSearch.Items.Clear();
            CBBrandSearch.Items.Add("All");
            while (reader.Read()) {
                CBBrandSearch.Items.Add((string)reader["brand_name"]);
            }
            reader.Close();
            con.Close();

            lblShowResultAmount.Visible = false;
            lblPage.Visible = false;
            btnFirstPage.Visible = false;
            btnLastPage.Visible = false;
            btnNextPage.Visible = false;
            btnPrevPage.Visible = false;
            btnGoToPage.Visible = false;
            txtPageInput.Visible = false;
            CBBrandSearch.SelectedIndex = 0;
            CBPriceOrder.SelectedIndex = 0;
            txtMinPrice.Text = "";
            txtMaxPrice.Text = "";

            // punch in
            if (GlobalVar.user_id > 0) {
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = $"INSERT INTO checkin (u_id) VALUES ({GlobalVar.user_id});";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void BtnRacquets_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Racquets");
            ProductsLoad(queryResult, 1);
        }

        private void BtnShoes_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Tennis Shoes");
            ProductsLoad(queryResult, 1);
        }

        private void BtnApparel_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Apparel");
            ProductsLoad(queryResult, 1);
        }

        private void BtnTennisBalls_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Tennis Balls");
            ProductsLoad(queryResult, 1);
        }

        private void BtnTennisBags_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Tennis Bags");
            ProductsLoad(queryResult, 1);
        }

        private void BtnGripAccessories_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Grip and Accessories");
            ProductsLoad(queryResult, 1);
        }

        private void BtnStrings_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(product => (string)product[2] == "Strings");
            ProductsLoad(queryResult, 1);
        }

        private void btnSignOut_Click(object sender, EventArgs e) {
            // clear current user info
            GlobalVar.user_id = 0;
            GlobalVar.user_name = "";
            GlobalVar.user_level = 0;
            GlobalVar.user_status = 0;
            GlobalVar.user_realName = "";
            GlobalVar.user_phone = "";
            GlobalVar.user_email = "";
            GlobalVar.user_address = "";
            GlobalVar.user_birthday = DateTime.Now;
            GlobalVar.NowSignIn = true;
            this.Close();
        }

        private void ProductsLoad(IEnumerable<ArrayList> result, int pageNum) {
            if (result.Count() == 0) {
                lblShowResultAmount.Text = "Show 0 - 0 of 0";
            }
            else {
                lblShowResultAmount.Text = $"Show {(pageNum - 1) * 10 + 1} - {Math.Min((pageNum - 1) * 10 + 10, result.Count())} of {result.Count()}";
            }
            txtPageInput.Text = pageNum.ToString();
            currPage = pageNum;
            pagesAmount = result.Count() / 10;
            if (result.Count() % 10 != 0) {
                pagesAmount++;
            }
            lblPage.Visible = true;
            btnFirstPage.Visible = true;
            btnLastPage.Visible = true;
            btnNextPage.Visible = true;
            btnPrevPage.Visible = true;
            btnGoToPage.Visible = true;
            txtPageInput.Visible = true;
            lblShowResultAmount.Visible = true;
            labelPrompt.Visible = false;
            // Load productInfo into listview
            imageListProducts.Images.Clear();
            imageListProducts.ImageSize = new Size(150, 150);
            listViewProductShow.Clear();
            listViewProductShow.View = View.LargeIcon;
            int itemIdx = (pageNum - 1) * 10 + 1;
            for (int i = 0; i < 10; i++) {
                if (itemIdx + i > result.Count()) {
                    break;
                }
                ArrayList currProduct = result.ElementAt(itemIdx + i - 1);
                string imagePath = GlobalVar.image_folder + @"\" + (string)currProduct[2] + @"\" + (string)currProduct[5];
                System.IO.FileStream fs = System.IO.File.OpenRead(imagePath);
                Image currImage = Image.FromStream(fs);
                imageListProducts.Images.Add(currImage);
                ListViewItem item = new ListViewItem();
                item.ImageIndex = imageListProducts.Images.Count - 1;
                item.Text = (string)currProduct[1];
                item.Font = new Font("Arial", 12, FontStyle.Bold);
                item.Tag = currProduct[0];
                listViewProductShow.Items.Add(item);
                fs.Close();
            }
            listViewProductShow.LargeImageList = imageListProducts;
        }

        private void listViewProductShow_ItemActivate(object sender, EventArgs e) {
            if (GlobalVar.user_id == 0) {
                MessageBox.Show("Please Sign in to Continue Shopping", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                ItemPage selectedProduct = new ItemPage();
                selectedProduct.product_id = (int)listViewProductShow.SelectedItems[0].Tag;
                selectedProduct.ShowDialog();
            }
        }

        private void btnGoToPage_Click(object sender, EventArgs e) {
            int gotoPage = 0;
            if (!Int32.TryParse(txtPageInput.Text, out gotoPage) || gotoPage > pagesAmount || gotoPage < 1) {
                MessageBox.Show("Invalid page input", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProductsLoad(queryResult, currPage);
            }
            else {
                ProductsLoad(queryResult, gotoPage);
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e) {
            if (currPage == pagesAmount) {
                MessageBox.Show("The current page is the last page.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                ProductsLoad(queryResult, ++currPage);
            }
        }

        private void btnPrevPage_Click(object sender, EventArgs e) {
            if (currPage == 1) {
                MessageBox.Show("The current page is the first page.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                ProductsLoad(queryResult, --currPage);
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e) {
            ProductsLoad(queryResult, pagesAmount);
        }

        private void btnFirstPage_Click(object sender, EventArgs e) {
            ProductsLoad(queryResult, 1);
        }

        private void btnCart_Click(object sender, EventArgs e) {
            Cart currCart = new Cart();
            currCart.ShowDialog();
        }

        private void btnMyOrders_Click(object sender, EventArgs e) {
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
        }

        private void btnProfile_Click(object sender, EventArgs e) {
            MemberProfile profileForm = new MemberProfile();
            profileForm.ShowDialog();
        }

        private void BtnSearch_Click(object sender, EventArgs e) {
            queryResult = productsAll.Where(p => p == p);
            int minPrice = 0;
            int maxPrice = Int32.MaxValue;
            bool allPrice = false;
            if (txtMaxPrice.Text.Length == 0 && txtMinPrice.Text.Length == 0) {
                allPrice = true;
            }
            if (allPrice == false && (!Int32.TryParse(txtMinPrice.Text, out minPrice) || !Int32.TryParse(txtMaxPrice.Text, out maxPrice) || minPrice > maxPrice)) {
                MessageBox.Show("The input price range is invalid", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                queryResult = productsAll.Where(p => (int)p[4] >= minPrice && (int)p[4] <= maxPrice);
                if (TxtNameSearch.Text.Length > 0) {
                    queryResult = queryResult.Where(p => p[1].ToString().Contains(TxtNameSearch.Text));
                }
                if (CBBrandSearch.SelectedIndex > 0) {
                    queryResult = queryResult.Where(p => (string)p[3] == CBBrandSearch.Items[CBBrandSearch.SelectedIndex].ToString());
                }
                if (CBPriceOrder.SelectedIndex == 1) {
                    queryResult = queryResult.OrderBy(p => (int)p[4]);
                }
                else if (CBPriceOrder.SelectedIndex == 2) {
                    queryResult = queryResult.OrderByDescending(p => (int)p[4]);
                }
                ProductsLoad(queryResult, 1);
            }
            
        }

        private void btnProductsManagement_Click(object sender, EventArgs e) {
            FormProductsManagement fm = new FormProductsManagement();
            this.Visible = false;
            fm.ShowDialog();
            this.Visible = true;

            // reload products and brands
            // load all on_shelf products from DB
            SqlConnection con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT p.*, b.brand_name, c.category_name, i.image_name " +
                    "FROM product AS p " +
                    "JOIN brand AS b " +
                    "ON p.brand = b.brand_id " +
                    "JOIN category AS c " +
                    "ON p.category = c.category_id " +
                    "JOIN (SELECT *, ROW_NUMBER() OVER (PARTITION BY product_id ORDER BY image_id) AS rowNum FROM product_image) AS i " +
                    "ON p.product_id = i.product_id " +
                    "WHERE on_shelf = 1 AND rowNum = 1;";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            productsAll.Clear();
            while (reader.Read()) {
                ArrayList productInfo = new ArrayList();
                productInfo.Add((int)reader["product_id"]);
                productInfo.Add((string)reader["product_name"]);
                productInfo.Add((string)reader["category_name"]);
                productInfo.Add((string)reader["brand_name"]);
                productInfo.Add((int)reader["price"]);
                productInfo.Add((string)reader["image_name"]);
                productsAll.Add(productInfo);
            }
            reader.Close();
            con.Close();

            // load brand combox items brand

            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM brand";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBBrandSearch.Items.Clear();
            CBBrandSearch.Items.Add("All");
            while (reader.Read()) {
                CBBrandSearch.Items.Add((string)reader["brand_name"]);
            }
            reader.Close();
            con.Close();
            listViewProductShow.Items.Clear();
            labelPrompt.Visible = true;
            lblShowResultAmount.Visible = false;
            lblPage.Visible = false;
            btnFirstPage.Visible = false;
            btnLastPage.Visible = false;
            btnNextPage.Visible = false;
            btnPrevPage.Visible = false;
            btnGoToPage.Visible = false;
            txtPageInput.Visible = false;
            CBBrandSearch.SelectedIndex = 0;
            CBPriceOrder.SelectedIndex = 0;
            txtMinPrice.Text = "";
            txtMaxPrice.Text = "";

        }

        private void btnOrdersReview_Click(object sender, EventArgs e) {
            OrdersReview or = new OrdersReview();
            or.ShowDialog();
        }
    }
}
