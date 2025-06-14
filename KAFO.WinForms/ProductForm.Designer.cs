namespace KAFO.WinForms
{
    partial class ProductForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            btnAdd = new Button();
            txtName = new TextBox();
            groupBox1 = new GroupBox();
            nudPrice = new NumericUpDown();
            label2 = new Label();
            groupBox1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize) nudPrice ).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 119);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(221, 35);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "ADD";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click +=  btnAdd_Click ;
            // 
            // txtName
            // 
            txtName.Location = new Point(71, 35);
            txtName.Name = "txtName";
            txtName.Size = new Size(162, 23);
            txtName.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nudPrice);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtName);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(274, 180);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Add Product";
            // 
            // nudPrice
            // 
            nudPrice.Location = new Point(71, 78);
            nudPrice.Name = "nudPrice";
            nudPrice.Size = new Size(162, 23);
            nudPrice.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 78);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 3;
            label2.Text = "Price";
            // 
            // ProductForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(274, 184);
            Controls.Add(groupBox1);
            Name = "ProductForm";
            Text = "ProductForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize) nudPrice ).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Button btnAdd;
        private TextBox txtName;
        private GroupBox groupBox1;
        private Label label2;
        private NumericUpDown nudPrice;
    }
}