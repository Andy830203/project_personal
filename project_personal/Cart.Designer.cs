namespace project_personal {
    partial class Cart {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.btnUpdateItemQuantity = new System.Windows.Forms.Button();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.lblItemsCount = new System.Windows.Forms.Label();
            this.txtNewQuantity = new System.Windows.Forms.TextBox();
            this.btnNewQuantityAdd1 = new System.Windows.Forms.Button();
            this.btnNewQuantitySub1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCart
            // 
            this.dgvCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCart.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCart.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvCart.Location = new System.Drawing.Point(26, 33);
            this.dgvCart.MultiSelect = false;
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.RowHeadersVisible = false;
            this.dgvCart.RowHeadersWidth = 51;
            this.dgvCart.RowTemplate.Height = 24;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.Size = new System.Drawing.Size(775, 419);
            this.dgvCart.TabIndex = 0;
            this.dgvCart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCart_CellClick);
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckOut.Location = new System.Drawing.Point(819, 474);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(191, 84);
            this.btnCheckOut.TabIndex = 1;
            this.btnCheckOut.Text = "Check Out";
            this.btnCheckOut.UseVisualStyleBackColor = true;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // btnUpdateItemQuantity
            // 
            this.btnUpdateItemQuantity.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateItemQuantity.Location = new System.Drawing.Point(819, 33);
            this.btnUpdateItemQuantity.Name = "btnUpdateItemQuantity";
            this.btnUpdateItemQuantity.Size = new System.Drawing.Size(191, 75);
            this.btnUpdateItemQuantity.TabIndex = 2;
            this.btnUpdateItemQuantity.Text = "Alter\r\nQuantity";
            this.btnUpdateItemQuantity.UseVisualStyleBackColor = true;
            this.btnUpdateItemQuantity.Click += new System.EventHandler(this.btnUpdateItemQuantity_Click);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteItem.Location = new System.Drawing.Point(819, 286);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(191, 75);
            this.btnDeleteItem.TabIndex = 3;
            this.btnDeleteItem.Text = "Delete\r\nItem";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(614, 500);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(47, 33);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "$0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(493, 500);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 33);
            this.label2.TabIndex = 5;
            this.label2.Text = "TOTAL:";
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(819, 377);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(191, 75);
            this.btnDeleteAll.TabIndex = 6;
            this.btnDeleteAll.Text = "Delete\rAll Items";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // lblItemsCount
            // 
            this.lblItemsCount.AutoSize = true;
            this.lblItemsCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemsCount.Location = new System.Drawing.Point(495, 464);
            this.lblItemsCount.Name = "lblItemsCount";
            this.lblItemsCount.Size = new System.Drawing.Size(64, 23);
            this.lblItemsCount.TabIndex = 7;
            this.lblItemsCount.Text = "0 item";
            // 
            // txtNewQuantity
            // 
            this.txtNewQuantity.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewQuantity.Location = new System.Drawing.Point(873, 121);
            this.txtNewQuantity.Name = "txtNewQuantity";
            this.txtNewQuantity.Size = new System.Drawing.Size(85, 30);
            this.txtNewQuantity.TabIndex = 8;
            this.txtNewQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNewQuantity.TextChanged += new System.EventHandler(this.txtNewQuantity_TextChanged);
            // 
            // btnNewQuantityAdd1
            // 
            this.btnNewQuantityAdd1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewQuantityAdd1.Location = new System.Drawing.Point(964, 121);
            this.btnNewQuantityAdd1.Name = "btnNewQuantityAdd1";
            this.btnNewQuantityAdd1.Size = new System.Drawing.Size(42, 30);
            this.btnNewQuantityAdd1.TabIndex = 9;
            this.btnNewQuantityAdd1.Text = "+";
            this.btnNewQuantityAdd1.UseVisualStyleBackColor = true;
            this.btnNewQuantityAdd1.Click += new System.EventHandler(this.btnNewQuantityAdd1_Click);
            // 
            // btnNewQuantitySub1
            // 
            this.btnNewQuantitySub1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewQuantitySub1.Location = new System.Drawing.Point(825, 121);
            this.btnNewQuantitySub1.Name = "btnNewQuantitySub1";
            this.btnNewQuantitySub1.Size = new System.Drawing.Size(42, 30);
            this.btnNewQuantitySub1.TabIndex = 10;
            this.btnNewQuantitySub1.Text = "-";
            this.btnNewQuantitySub1.UseVisualStyleBackColor = true;
            this.btnNewQuantitySub1.Click += new System.EventHandler(this.btnNewQuantitySub1_Click);
            // 
            // Cart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1032, 587);
            this.Controls.Add(this.btnNewQuantitySub1);
            this.Controls.Add(this.btnNewQuantityAdd1);
            this.Controls.Add(this.txtNewQuantity);
            this.Controls.Add(this.lblItemsCount);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnDeleteItem);
            this.Controls.Add(this.btnUpdateItemQuantity);
            this.Controls.Add(this.btnCheckOut);
            this.Controls.Add(this.dgvCart);
            this.Name = "Cart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cart";
            this.Load += new System.EventHandler(this.Cart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Button btnUpdateItemQuantity;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Label lblItemsCount;
        private System.Windows.Forms.TextBox txtNewQuantity;
        private System.Windows.Forms.Button btnNewQuantityAdd1;
        private System.Windows.Forms.Button btnNewQuantitySub1;
    }
}