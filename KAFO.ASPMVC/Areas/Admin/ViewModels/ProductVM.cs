using KAFO.Domain.Products;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class ProductVM
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public string? ImageUrl { set; get; }
        public decimal SellingPrice { set; get; }
        public decimal StockQuantity { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal BoxPurchasePrice { set; get; }
        public int BoxQuantity { set; get; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { set; get; }
    }
    public static class ProductVMHelper
    {
        public static Product ToProduct(this ProductVM productVM, string? ImageUrl = null)
        {
            return new Product
            {
                Id = productVM.Id ?? 0,
                Name = productVM.Name,
                ImageUrl = ImageUrl ?? productVM.ImageUrl,
                SellingPrice = productVM.SellingPrice,
                StockQuantity = productVM.StockQuantity,
                AveragePurchasePrice = productVM.AveragePurchasePrice,
                BoxPurchasePrice = productVM.BoxPurchasePrice,
                BoxQuantity = productVM.BoxQuantity,
                IsActive = productVM.IsActive,
                CategoryId = productVM.CategoryId,
                Category = productVM.Category
            };
        }
        public static Product UpdateProduct(this ProductVM productVM, Product product)
        {
            product.Name = productVM.Name;
            product.ImageUrl = productVM.ImageUrl;
            product.SellingPrice = productVM.SellingPrice;
            product.StockQuantity = productVM.StockQuantity;
            product.AveragePurchasePrice = productVM.AveragePurchasePrice;
            product.BoxPurchasePrice = productVM.BoxPurchasePrice;
            product.BoxQuantity = productVM.BoxQuantity;
            product.IsActive = productVM.IsActive;
            product.CategoryId = productVM.CategoryId;
            product.Category = productVM.Category;
            return product;
        }

        public static ProductVM ToProductVM(this Product product)
        {
            return new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                SellingPrice = product.SellingPrice,
                StockQuantity = product.StockQuantity,
                AveragePurchasePrice = product.AveragePurchasePrice,
                BoxPurchasePrice = product.BoxPurchasePrice,
                BoxQuantity = product.BoxQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                Category = product.Category
            };
        }
    }
}
