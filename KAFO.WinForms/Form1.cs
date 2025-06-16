using KAFO.BLL.Managers;
using KAFO.Domain.Products;

namespace KAFO.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Product> products;
        List<Product> InvoiceProducts = [];
        private void Form1_Load(object sender, EventArgs e)
        {
            FormRefresh();
        }

        private void DynamicButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(labCreatedAt.Text))
                labCreatedAt.Text = DateTime.Now.ToString();

            Button obj = (Button) sender;
            Product p = (Product) obj.Tag;
            if (InvoiceProducts.Contains(p))
            {
                p.StockQuantity++;
            }
            else
            {
                //add to the invoice 
                InvoiceProducts.Add(p);
            }

            InvoiceFlowRefresh();
            //update total price to pay
            var totalPrice = products.Sum(x => x.SellingPrice * x.StockQuantity);
            btnPay.Text = $"Pay And Close - {totalPrice}";
        }

        private void InvoiceFlowRefresh()
        {
            InvoiceFlow.Controls.Clear();
            foreach (var item in InvoiceProducts)
            {
                AddToInvoice(item);
            }
        }

        private void AddToInvoice(Product p)
        {
            if (p.StockQuantity < 1) p.StockQuantity++;
            Button dynamicButton = new Button();
            dynamicButton.Text = $"{p.Name} - {p.SellingPrice}- Q:{p.StockQuantity}";
            dynamicButton.Width = 200;
            dynamicButton.Dock = DockStyle.Top;
            dynamicButton.Tag = p;
            dynamicButton.Height = 40;
            dynamicButton.Margin = new Padding(5); // Add spacing
            dynamicButton.Click += InvoiceDynamicButton_Click;

            InvoiceFlow.Controls.Add(dynamicButton);
        }

        private void InvoiceDynamicButton_Click(object? sender, EventArgs e)
        {
            Button obj = (Button) sender;
            Product p = (Product) obj.Tag;
            p.StockQuantity--;
            if (p.StockQuantity < 1) InvoiceProducts.Remove(p);
            InvoiceFlowRefresh();

            var totalPrice = products.Sum(x => x.SellingPrice * x.StockQuantity);
            btnPay.Text = $"Pay And Close - {totalPrice}";
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            new ProductForm().ShowDialog();
            FormRefresh();
        }

        private void FormRefresh()
        {
        //    products = ProductManager.GetAll();
            Producsflow.Controls.Clear();
            foreach (var p in products)
            {
                Button dynamicButton = new Button();
                dynamicButton.Text = $"{p.Name} - {p.SellingPrice}";
                dynamicButton.Width = 100;
                dynamicButton.Tag = p;
                dynamicButton.Height = 40;
                dynamicButton.Margin = new Padding(5); // Add spacing
                dynamicButton.Click += DynamicButton_Click;

                Producsflow.Controls.Add(dynamicButton);
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            var totalPrice = products.Sum(x => x.SellingPrice * x.StockQuantity);
            MessageBox.Show($"The Total Price is: {totalPrice}");
        }
    }
}
