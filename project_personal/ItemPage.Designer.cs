namespace project_personal {
    partial class ItemPage {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.pBoxItem = new System.Windows.Forms.PictureBox();
            this.cBoxSize = new System.Windows.Forms.ComboBox();
            this.lblItemName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnQuantityAdd1 = new System.Windows.Forms.Button();
            this.btnQuantitySub1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbllInStock = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTPrice = new System.Windows.Forms.Label();
            this.lblUPrice = new System.Windows.Forms.Label();
            this.BtnAddToCart = new System.Windows.Forms.Button();
            this.BtnBack = new System.Windows.Forms.Button();
            this.imageListCurrItem = new System.Windows.Forms.ImageList(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.btnPrevImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxItem)).BeginInit();
            this.SuspendLayout();
            // 
            // pBoxItem
            // 
            this.pBoxItem.Location = new System.Drawing.Point(12, 21);
            this.pBoxItem.Name = "pBoxItem";
            this.pBoxItem.Size = new System.Drawing.Size(256, 256);
            this.pBoxItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxItem.TabIndex = 0;
            this.pBoxItem.TabStop = false;
            // 
            // cBoxSize
            // 
            this.cBoxSize.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBoxSize.FormattingEnabled = true;
            this.cBoxSize.Location = new System.Drawing.Point(432, 35);
            this.cBoxSize.Name = "cBoxSize";
            this.cBoxSize.Size = new System.Drawing.Size(133, 32);
            this.cBoxSize.TabIndex = 1;
            this.cBoxSize.SelectedIndexChanged += new System.EventHandler(this.cBoxSize_SelectedIndexChanged);
            // 
            // lblItemName
            // 
            this.lblItemName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemName.Location = new System.Drawing.Point(8, 311);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(260, 85);
            this.lblItemName.TabIndex = 2;
            this.lblItemName.Text = "ItemName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(357, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(315, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Quantity:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(455, 84);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(82, 30);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.Text = "0";
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
            // 
            // btnQuantityAdd1
            // 
            this.btnQuantityAdd1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuantityAdd1.Location = new System.Drawing.Point(541, 82);
            this.btnQuantityAdd1.Name = "btnQuantityAdd1";
            this.btnQuantityAdd1.Size = new System.Drawing.Size(40, 34);
            this.btnQuantityAdd1.TabIndex = 6;
            this.btnQuantityAdd1.Text = "+";
            this.btnQuantityAdd1.UseVisualStyleBackColor = true;
            this.btnQuantityAdd1.Click += new System.EventHandler(this.btnQuantityAdd1_Click);
            // 
            // btnQuantitySub1
            // 
            this.btnQuantitySub1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuantitySub1.Location = new System.Drawing.Point(411, 82);
            this.btnQuantitySub1.Name = "btnQuantitySub1";
            this.btnQuantitySub1.Size = new System.Drawing.Size(40, 34);
            this.btnQuantitySub1.TabIndex = 7;
            this.btnQuantitySub1.Text = "-";
            this.btnQuantitySub1.UseVisualStyleBackColor = true;
            this.btnQuantitySub1.Click += new System.EventHandler(this.btnQuantitySub1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(304, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Unit Price:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(295, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Total Price:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(319, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "In Stock:";
            // 
            // lbllInStock
            // 
            this.lbllInStock.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllInStock.Location = new System.Drawing.Point(451, 139);
            this.lbllInStock.Name = "lbllInStock";
            this.lbllInStock.Size = new System.Drawing.Size(94, 24);
            this.lbllInStock.TabIndex = 11;
            this.lbllInStock.Text = "0";
            this.lbllInStock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(419, 238);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 24);
            this.label9.TabIndex = 14;
            this.label9.Text = "$";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(419, 189);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 24);
            this.label10.TabIndex = 15;
            this.label10.Text = "$";
            // 
            // lblTPrice
            // 
            this.lblTPrice.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTPrice.Location = new System.Drawing.Point(451, 238);
            this.lblTPrice.Name = "lblTPrice";
            this.lblTPrice.Size = new System.Drawing.Size(94, 24);
            this.lblTPrice.TabIndex = 16;
            this.lblTPrice.Text = "0";
            this.lblTPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUPrice
            // 
            this.lblUPrice.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUPrice.Location = new System.Drawing.Point(451, 189);
            this.lblUPrice.Name = "lblUPrice";
            this.lblUPrice.Size = new System.Drawing.Size(94, 24);
            this.lblUPrice.TabIndex = 17;
            this.lblUPrice.Text = "0";
            this.lblUPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnAddToCart
            // 
            this.BtnAddToCart.BackColor = System.Drawing.Color.Green;
            this.BtnAddToCart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAddToCart.ForeColor = System.Drawing.Color.White;
            this.BtnAddToCart.Location = new System.Drawing.Point(301, 287);
            this.BtnAddToCart.Name = "BtnAddToCart";
            this.BtnAddToCart.Size = new System.Drawing.Size(276, 60);
            this.BtnAddToCart.TabIndex = 18;
            this.BtnAddToCart.Text = "Add To Cart";
            this.BtnAddToCart.UseVisualStyleBackColor = false;
            this.BtnAddToCart.Click += new System.EventHandler(this.BtnAddToCart_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBack.Location = new System.Drawing.Point(301, 353);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(276, 43);
            this.BtnBack.TabIndex = 20;
            this.BtnBack.Text = "Continue Shopping";
            this.BtnBack.UseVisualStyleBackColor = true;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // imageListCurrItem
            // 
            this.imageListCurrItem.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListCurrItem.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListCurrItem.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(8, 419);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(573, 129);
            this.lblDescription.TabIndex = 21;
            this.lblDescription.Text = "description";
            // 
            // btnNextImage
            // 
            this.btnNextImage.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImage.Location = new System.Drawing.Point(167, 283);
            this.btnNextImage.Name = "btnNextImage";
            this.btnNextImage.Size = new System.Drawing.Size(46, 25);
            this.btnNextImage.TabIndex = 22;
            this.btnNextImage.Text = ">";
            this.btnNextImage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNextImage.UseVisualStyleBackColor = true;
            this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
            // 
            // btnPrevImage
            // 
            this.btnPrevImage.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevImage.Location = new System.Drawing.Point(58, 283);
            this.btnPrevImage.Name = "btnPrevImage";
            this.btnPrevImage.Size = new System.Drawing.Size(46, 25);
            this.btnPrevImage.TabIndex = 23;
            this.btnPrevImage.Text = "<";
            this.btnPrevImage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrevImage.UseVisualStyleBackColor = true;
            this.btnPrevImage.Click += new System.EventHandler(this.btnPrevImage_Click);
            // 
            // ItemPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(595, 557);
            this.Controls.Add(this.btnPrevImage);
            this.Controls.Add(this.btnNextImage);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.BtnBack);
            this.Controls.Add(this.BtnAddToCart);
            this.Controls.Add(this.lblUPrice);
            this.Controls.Add(this.lblTPrice);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbllInStock);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnQuantitySub1);
            this.Controls.Add(this.btnQuantityAdd1);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.cBoxSize);
            this.Controls.Add(this.pBoxItem);
            this.Name = "ItemPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemPage";
            this.Load += new System.EventHandler(this.ItemPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxItem;
        private System.Windows.Forms.ComboBox cBoxSize;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnQuantityAdd1;
        private System.Windows.Forms.Button btnQuantitySub1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbllInStock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTPrice;
        private System.Windows.Forms.Label lblUPrice;
        private System.Windows.Forms.Button BtnAddToCart;
        private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.ImageList imageListCurrItem;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnNextImage;
        private System.Windows.Forms.Button btnPrevImage;
    }
}