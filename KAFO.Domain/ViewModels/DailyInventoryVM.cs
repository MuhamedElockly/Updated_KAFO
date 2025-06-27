using System.Collections.Generic;

namespace KAFO.Domain.ViewModels
{
    public class DailyInventoryVM
    {
        public List<SellerInventoryVM> SellersInventory { get; set; } = new List<SellerInventoryVM>();
        public List<ProductRemainVM> ProductsRemain { get; set; } = new List<ProductRemainVM>();
    }
} 