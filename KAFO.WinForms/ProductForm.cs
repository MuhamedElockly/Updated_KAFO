using KAFO.BLL.Managers;
using KAFO.Domain.Products;

namespace KAFO.WinForms
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductManager.Add(new Product(txtName.Text, 0, nudPrice.Value));
            txtName.Text = "";
            nudPrice.Value = 0;
        }
    }
}
