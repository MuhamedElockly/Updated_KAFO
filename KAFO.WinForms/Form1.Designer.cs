namespace KAFO.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            InvoiceFlow = new FlowLayoutPanel();
            labCreatedAt = new Label();
            btnPay = new Button();
            groupBox2 = new GroupBox();
            btnProduct = new Button();
            Producsflow = new FlowLayoutPanel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(InvoiceFlow);
            groupBox1.Controls.Add(labCreatedAt);
            groupBox1.Controls.Add(btnPay);
            groupBox1.Dock = DockStyle.Right;
            groupBox1.Location = new Point(570, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(231, 391);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Invoice";
            // 
            // InvoiceFlow
            // 
            InvoiceFlow.Anchor =     AnchorStyles.Top  |  AnchorStyles.Bottom   |  AnchorStyles.Left   |  AnchorStyles.Right ;
            InvoiceFlow.BackColor = SystemColors.ActiveCaption;
            InvoiceFlow.Location = new Point(6, 73);
            InvoiceFlow.Name = "InvoiceFlow";
            InvoiceFlow.Size = new Size(219, 267);
            InvoiceFlow.TabIndex = 2;
            // 
            // labCreatedAt
            // 
            labCreatedAt.AutoSize = true;
            labCreatedAt.Location = new Point(6, 22);
            labCreatedAt.Name = "labCreatedAt";
            labCreatedAt.Size = new Size(0, 15);
            labCreatedAt.TabIndex = 1;
            // 
            // btnPay
            // 
            btnPay.Dock = DockStyle.Bottom;
            btnPay.Location = new Point(3, 346);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(225, 42);
            btnPay.TabIndex = 0;
            btnPay.Text = "Pay And Close";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click +=  btnPay_Click ;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnProduct);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(570, 73);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Controls";
            // 
            // btnProduct
            // 
            btnProduct.Location = new Point(12, 22);
            btnProduct.Name = "btnProduct";
            btnProduct.Size = new Size(111, 45);
            btnProduct.TabIndex = 0;
            btnProduct.Text = "Products";
            btnProduct.UseVisualStyleBackColor = true;
            btnProduct.Click +=  btnProduct_Click ;
            // 
            // Producsflow
            // 
            Producsflow.BackColor = SystemColors.Control;
            Producsflow.Dock = DockStyle.Fill;
            Producsflow.Location = new Point(0, 73);
            Producsflow.Name = "Producsflow";
            Producsflow.Size = new Size(570, 318);
            Producsflow.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(801, 391);
            Controls.Add(Producsflow);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            Load +=  Form1_Load ;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox1;
        private Button btnPay;
        private GroupBox groupBox2;
        private Button btnProduct;
        private FlowLayoutPanel Producsflow;
        private Label labCreatedAt;
        private FlowLayoutPanel InvoiceFlow;
    }
}
