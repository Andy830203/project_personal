using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_personal {
    public partial class FormProductsManagement : Form {
        string query = "";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        int[] inStockMemo;
        List<Image> imageMemo = new List<Image>();
        List<string> extensionMemo = new List<string>();
        int categoryPrev = 0;
        public FormProductsManagement() {
            InitializeComponent();
        }

        private void FormProductsManagement_Load(object sender, EventArgs e) {
            // category
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM category;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBCategorySelect.Items.Add("No category");
            while (reader.Read()) {
                CBCategorySelect.Items.Add((string)reader["category_name"]);
            }
            reader.Close();
            con.Close();
            CBCategorySelect.SelectedIndex = 0;

            // size
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM size;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBSizeSelect.Items.Clear();
            while (reader.Read()) {
                CBSizeSelect.Items.Add((string)reader["size_name"]);
            }
            reader.Close();
            con.Close();
            inStockMemo = new int[CBSizeSelect.Items.Count];
            CBSizeSelect.SelectedIndex = 15;

            // brand
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM brand;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBBrandSelect.Items.Clear();
            CBBrandSelect.Items.Add("No brand");
            while (reader.Read()) {
                CBBrandSelect.Items.Add((string)reader["brand_name"]);
            }
            reader.Close();
            con.Close();

            gBoxAddNewProduct.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (btnAddOrEdit.Text == "Add") {
                bool valid = true;
                int price = 0;
                if (txtProductName.Text.Length == 0) {
                    MessageBox.Show("The new product name is required", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valid = false;
                }
                if (valid && checkAddNewBrand.Checked && txtNewBrand.Text.Length <= 0) {
                    MessageBox.Show("If You want to add a new brand, a new brand name is required.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valid = false;
                }
                if (valid) {
                    if (txtPrice.Text.Length <= 0) {
                        MessageBox.Show("The price of the new product is required.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        valid = false;
                    }
                    else if (!Int32.TryParse(txtPrice.Text, out price)) {
                        MessageBox.Show("The input price of the new product is invalid.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        valid = false;
                    }
                }
                int onShelf = 0;
                if (checkOnShelf.Checked) {
                    onShelf = 1;
                }
                // insert product, size_product, product_image tables;
                if (valid) {
                    int productBrand = 0;
                    if (checkAddNewBrand.Checked) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "INSERT INTO brand (brand_Name) " +
                                $"VALUES(@brandName);";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@brandName", txtNewBrand.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        productBrand = CBBrandSelect.Items.Count;
                    }
                    else {
                        productBrand = CBBrandSelect.SelectedIndex;
                    }
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "INSERT INTO product (product_name, price, on_shelf, description) " +
                            $"VALUES(@productName, {price}, {onShelf}, @description);";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@productName", $@"{txtProductName.Text}");
                    cmd.Parameters.AddWithValue("@description", $@"{txtDescription.Text}");
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // update category and brand if needed
                    if (CBCategorySelect.SelectedIndex > 0) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "UPDATE product " +
                                $"SET category = {CBCategorySelect.SelectedIndex} " +
                                $"WHERE product_name = @productName";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@productName", txtProductName.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    if (checkAddNewBrand.Checked || CBBrandSelect.SelectedIndex > 0) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "UPDATE  product " +
                                $"SET brand = {productBrand} " +
                                $"WHERE product_name = @productName";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@productName", txtProductName.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    //get the current product id first
                    int currPID = 0;
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "SELECT MAX(product_id) FROM product;";
                    cmd = new SqlCommand(query, con);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    currPID = (int)reader[0];
                    reader.Close();
                    con.Close();

                    // insert into size_product table
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "INSERT INTO size_product (p_id, s_id, in_stock) VALUES ";
                    bool firstValue = true;
                    for (int i = 0; i < CBSizeSelect.Items.Count; i++) {
                        if (inStockMemo[i] > 0) {
                            if (!firstValue) {
                                query += ", ";
                            }
                            query += $"({currPID}, {i + 1}, {inStockMemo[i]})";
                            firstValue = false;
                        }
                    }
                    query += ";";
                    if (!firstValue) { // There is at least one instock need to be added
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                    // insert into product_image and save the images into images folder
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "INSERT INTO product_image (product_id, image_name) VALUES ";
                    bool firstPicture = true;
                    for (int i = 0; i < imageMemo.Count; i++) {
                        Image im = imageMemo[i];
                        string path = $@"{GlobalVar.image_folder}\{(string)CBCategorySelect.Items[CBCategorySelect.SelectedIndex]}\{txtProductName.Text.Trim().Replace(' ', '_')}_{i + 1}{extensionMemo[i]}";
                        Console.WriteLine(path);
                        im.Save(path);
                        if (!firstPicture) {
                            query += ", ";
                        }
                        else {
                            firstPicture = false;
                        }
                        string currPName = txtProductName.Text.Trim().Replace(' ', '_') + "_" + $"{i + 1}" + extensionMemo[i];
                        query += $"({currPID}, '{currPName}') ";
                    }
                    query += ";";
                    Console.WriteLine(query);
                    if (!firstPicture) {
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    MessageBox.Show("The product is added successfully.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // all done, if add new brand this time, refresh brand combobox
                if (checkAddNewBrand.Checked) {
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "SELECT * " +
                            "FROM brand;";
                    cmd = new SqlCommand(query, con);
                    reader = cmd.ExecuteReader();
                    CBBrandSelect.Items.Clear();
                    CBBrandSelect.Items.Add("No brand");
                    while (reader.Read()) {
                        CBBrandSelect.Items.Add((string)reader["brand_name"]);
                    }
                    reader.Close();
                    con.Close();
                }
                inStockMemo = new int[inStockMemo.Length];
                imageMemo.Clear();
                extensionMemo.Clear();
                listBoxImageAdd.Items.Clear();

                // clear all 
                txtProductName.Clear();
                txtNewBrand.Clear();
                CBCategorySelect.SelectedIndex = 0;
                txtPrice.Clear();
                checkOnShelf.Checked = false;
                CBSizeSelect.SelectedIndex = 15;
                txtInStock.Clear();
                CBBrandSelect.SelectedIndex = 0;
                txtDescription.Clear();
            }
            // Edit
            else {
                bool valid = true;
                int price = 0;
                if (txtProductName.Text.Length == 0) {
                    MessageBox.Show("The new product name is required", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valid = false;
                }
                if (valid && checkAddNewBrand.Checked && txtNewBrand.Text.Length <= 0) {
                    MessageBox.Show("If You want to add a new brand, a new brand name is required.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valid = false;
                }
                if (valid) {
                    if (txtPrice.Text.Length <= 0) {
                        MessageBox.Show("The price of the new product is required.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        valid = false;
                    }
                    else if (!Int32.TryParse(txtPrice.Text, out price)) {
                        MessageBox.Show("The input price of the new product is invalid.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        valid = false;
                    }
                }
                int onShelf = 0;
                if (checkOnShelf.Checked) {
                    onShelf = 1;
                }
                // update product, size_product tables;
                if (valid) {
                    int productBrand = 0;
                    if (checkAddNewBrand.Checked) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "INSERT INTO brand (brand_Name) " +
                                $"VALUES(@brandName);";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@brandName", $@"{txtNewBrand.Text}");
                        cmd.ExecuteNonQuery();
                        con.Close();
                        productBrand = CBBrandSelect.Items.Count;
                    }
                    else {
                        productBrand = CBBrandSelect.SelectedIndex;
                    }
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "UPDATE product " +
                            $"SET product_name = '{txtProductName.Text}', price = {price}, on_shelf = {onShelf}, description = '{txtDescription.Text}', brand = {productBrand} " +
                            $"WHERE product_id = {txtIDInput.Text};";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (CBCategorySelect.SelectedIndex > 0) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "UPDATE product " +
                                $"SET category = {CBCategorySelect.SelectedIndex} " +
                                $"WHERE product_name = @productName";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@productName", txtProductName.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //  delete images from local folder
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "SELECT image_name " +
                            "FROM product_image " +
                            "WHERE product_id = @ID;";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", txtIDInput.Text);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        string path = $@"{GlobalVar.image_folder}\" + (string)CBCategorySelect.Items[categoryPrev] +
                                      @"\" + (string)reader["image_name"];
                        File.Delete(path);
                    }
                    reader.Close();
                    con.Close();
                    // delete images path from DB
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "DELETE " +
                            "FROM product_image " +
                            "WHERE product_id = " + txtIDInput.Text + ";";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // Add images from imageMemo
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "INSERT INTO product_image (product_id, image_name) VALUES ";
                    bool firstPicture = true;
                    for (int i = 0; i < imageMemo.Count; i++) {
                        Image im = imageMemo[i];
                        string path = $@"{GlobalVar.image_folder}\{(string)CBCategorySelect.Items[CBCategorySelect.SelectedIndex]}\{txtProductName.Text.Trim().Replace(' ', '_')}_{i + 1}{extensionMemo[i]}";
                        Console.WriteLine(path);
                        im.Save(path);
                        if (!firstPicture) {
                            query += ", ";
                        }
                        else {
                            firstPicture = false;
                        }
                        string currPName = txtProductName.Text.Trim().Replace(' ', '_') + "_" + $"{i + 1}" + extensionMemo[i];
                        query += $"({txtIDInput.Text.Trim()}, '{currPName}') ";
                    }
                    query += ";";
                    Console.WriteLine(query);
                    if (!firstPicture) {
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                    // size_product update
                    for (int i = 0; i < CBSizeSelect.Items.Count; i++) {
                        bool alreadyInDB = false;
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "SELECT * " +
                                "FROM size_product " +
                                $"WHERE p_id = {txtIDInput.Text} AND s_id = {i + 1};";
                        cmd = new SqlCommand(query, con);
                        reader = cmd.ExecuteReader();
                        if (reader.Read()) {
                            alreadyInDB = true;
                        }
                        else {
                            alreadyInDB = false;
                        }
                        reader.Close();
                        con.Close();
                        if (alreadyInDB) {
                            con = new SqlConnection(GlobalVar.strDBConnectionString);
                            con.Open();
                            query = "UPDATE size_product " +
                                    $"SET in_stock = {inStockMemo[i]} " +
                                    $"WHERE p_id = {txtIDInput.Text} AND s_id = {i + 1};";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else if (inStockMemo[i] > 0) {
                            con = new SqlConnection(GlobalVar.strDBConnectionString);
                            con.Open();
                            query = "INSERT INTO size_product (p_id, s_id, in_stock) VALUES " +
                                    $"({txtIDInput.Text}, {i + 1}, {inStockMemo[i]})";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    if (checkAddNewBrand.Checked) {
                        con = new SqlConnection(GlobalVar.strDBConnectionString);
                        con.Open();
                        query = "SELECT * " +
                                "FROM brand;";
                        cmd = new SqlCommand(query, con);
                        reader = cmd.ExecuteReader();
                        CBBrandSelect.Items.Clear();
                        CBBrandSelect.Items.Add("No brand");
                        while (reader.Read()) {
                            CBBrandSelect.Items.Add((string)reader["brand_name"]);
                        }
                        reader.Close();
                        con.Close();
                    }
                    MessageBox.Show("The product is edited successfully.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    inStockMemo = new int[inStockMemo.Length];
                    imageMemo.Clear();
                    extensionMemo.Clear();
                    listBoxImageAdd.Items.Clear();

                    // clear all 
                    txtProductName.Clear();
                    txtNewBrand.Clear();
                    CBCategorySelect.SelectedIndex = 0;
                    txtPrice.Clear();
                    checkOnShelf.Checked = false;
                    CBSizeSelect.SelectedIndex = 15;
                    txtInStock.Clear();
                    CBBrandSelect.SelectedIndex = 0;
                    txtDescription.Clear();
                    txtIDInput.Clear();
                    txtIDInput.ReadOnly = false;
                }
            }
        }

        private void addInStock_Click(object sender, EventArgs e) {
            int inputInstock = 0;
            if (txtInStock.Text.Length == 0 || !Int32.TryParse(txtInStock.Text, out inputInstock)) {
                MessageBox.Show("The input in stock quantity is invalid.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                inStockMemo[CBSizeSelect.SelectedIndex] = inputInstock;
                MessageBox.Show("The in stock quantity is added successfully.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image file(.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK) {
                System.IO.FileStream fs = System.IO.File.OpenRead(ofd.FileName);
                imageMemo.Add(Image.FromStream(fs));
                extensionMemo.Add(System.IO.Path.GetExtension(ofd.SafeFileName).ToLower());
                listBoxImageAdd.Items.Add(ofd.FileName);
                fs.Close();
            }
        }

        private void btnDropImage_Click(object sender, EventArgs e) {
            if (listBoxImageAdd.SelectedIndex >= 0) {
                imageMemo.RemoveAt(listBoxImageAdd.SelectedIndex);
                extensionMemo.RemoveAt(listBoxImageAdd.SelectedIndex);
                listBoxImageAdd.Items.RemoveAt(listBoxImageAdd.SelectedIndex);
                listBoxImageAdd.SelectedItems.Clear();
            }
        }

        private void CBSizeSelect_SelectedIndexChanged(object sender, EventArgs e) {
            txtInStock.Text = $"{inStockMemo[CBSizeSelect.SelectedIndex]}";
        }

        private void btnEditProduct_Click(object sender, EventArgs e) {
            gBoxAddNewProduct.Visible = true;
            btnShowData.Visible = true;
            loadEdit();
            btnAddOrEdit.Text = "Edit";
            addInStock.Text = "Edit";
        }
        private void loadEdit() {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM brand;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBBrandSelect.Items.Clear();
            CBBrandSelect.Items.Add("No brand");
            while (reader.Read()) {
                CBBrandSelect.Items.Add((string)reader["brand_name"]);
            }
            reader.Close();
            con.Close();

            // clear all 
            inStockMemo = new int[inStockMemo.Length];
            imageMemo.Clear();
            extensionMemo.Clear();
            listBoxImageAdd.Items.Clear();
            txtProductName.Clear();
            txtNewBrand.Clear();
            CBCategorySelect.SelectedIndex = 0;
            txtPrice.Clear();
            checkOnShelf.Checked = false;
            CBSizeSelect.SelectedIndex = 15;
            txtInStock.Clear();
            CBBrandSelect.SelectedIndex = 0;
            txtDescription.Clear();

            lblProductID.Visible = true;
            txtIDInput.Visible = true;
            txtIDInput.Clear();
            txtIDInput.ReadOnly = false;
            lblIDRequired.Visible = true;
        }

        private void btnAddNewProduct_Click(object sender, EventArgs e) {
            gBoxAddNewProduct.Visible = true;
            btnAddOrEdit.Visible = true;
            btnShowData.Visible = false;
            loadAdd();
            btnAddOrEdit.Text = "Add";
            addInStock.Text = "Add";
        }
        private void loadAdd() {
            con = new SqlConnection(GlobalVar.strDBConnectionString);
            con.Open();
            query = "SELECT * " +
                    "FROM brand;";
            cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            CBBrandSelect.Items.Clear();
            CBBrandSelect.Items.Add("No brand");
            while (reader.Read()) {
                CBBrandSelect.Items.Add((string)reader["brand_name"]);
            }
            reader.Close();
            con.Close();

            // clear all 
            inStockMemo = new int[inStockMemo.Length];
            imageMemo.Clear();
            extensionMemo.Clear();
            listBoxImageAdd.Items.Clear();
            txtProductName.Clear();
            txtNewBrand.Clear();
            CBCategorySelect.SelectedIndex = 0;
            txtPrice.Clear();
            checkOnShelf.Checked = false;
            CBSizeSelect.SelectedIndex = 15;
            txtInStock.Clear();
            CBBrandSelect.SelectedIndex = 0;
            txtDescription.Clear();
            lblProductID.Visible = false;
            txtIDInput.Visible = false;
            lblIDRequired.Visible = false;
            lblNameRequired.Visible = true;
            lblPriceRequired.Visible = true;
        }

        private void btnShowData_Click(object sender, EventArgs e) {
            int ID = 0;
            txtProductName.Clear();
            checkAddNewBrand.Checked = false;
            txtNewBrand.Clear();
            txtPrice.Clear();
            CBCategorySelect.SelectedIndex = 0;
            CBSizeSelect.SelectedIndex = 15;
            txtInStock.Clear();
            checkOnShelf.Checked = false;
            txtDescription.Clear();
            listBoxImageAdd.Items.Clear();
            imageMemo.Clear();
            extensionMemo.Clear();
            if (!Int32.TryParse(txtIDInput.Text, out ID)) {
                MessageBox.Show("The input ID is invalid.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDInput.Clear();
            }
            else {
                bool found = false;
                con = new SqlConnection(GlobalVar.strDBConnectionString);
                con.Open();
                query = "SELECT * " +
                        "FROM product " +
                        "WHERE product_id = @ID;";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", $"{ID}");
                reader = cmd.ExecuteReader();
                if (!reader.Read()) {
                    MessageBox.Show("The input product ID is not found.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIDInput.Clear();
                }
                else {
                    found = true;
                    txtProductName.Text = (string)reader["product_name"];
                    CBBrandSelect.SelectedIndex = (int)reader["brand"];
                    txtPrice.Text = $"{(int)reader["price"]}";
                    CBCategorySelect.SelectedIndex = (int)reader["category"];
                    checkOnShelf.Checked = ((bool)reader["on_shelf"] == true);
                    txtDescription.Text = (string)reader["description"];
                }
                reader.Close();
                con.Close();
                if (found) {
                    // size
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "SELECT * " +
                            "FROM size_product AS sp " +
                            "WHERE p_id = @ID;";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", $"{ID}");
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        inStockMemo[(int)reader["s_id"] - 1] = (int)reader["in_stock"]; 
                    }
                    reader.Close();
                    con.Close();

                    // image
                    con = new SqlConnection(GlobalVar.strDBConnectionString);
                    con.Open();
                    query = "SELECT * " +
                            "FROM product_image AS pi " +
                            "WHERE product_id = @ID;";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", $"{ID}");
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        listBoxImageAdd.Items.Add((string)reader["image_name"]);
                    }
                    reader.Close();
                    con.Close();
                    // load ths images belonging to this product into list 
                    for (int i = 0; i < listBoxImageAdd.Items.Count; i++) {
                        string path = $@"{GlobalVar.image_folder}\{(string)CBCategorySelect.Items[CBCategorySelect.SelectedIndex]}\{(string)listBoxImageAdd.Items[i]}";
                        System.IO.FileStream fs = System.IO.File.OpenRead(path);
                        imageMemo.Add(Image.FromStream(fs));
                        extensionMemo.Add(System.IO.Path.GetExtension((string)listBoxImageAdd.Items[i]).ToLower());
                        fs.Close();
                    }
                    categoryPrev = CBCategorySelect.SelectedIndex;
                    txtIDInput.ReadOnly = true;
                }
            }
        }
    }
}
