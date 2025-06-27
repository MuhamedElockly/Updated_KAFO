using System;

namespace KAFO.Domain.ViewModels
{
    public class ProductRemainVM
    {
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public int RemainBox { get; set; }
        public int RemainPlus { get; set; }
        public int TotalRemain { get; set; }
        public int ItemsPerBox { get; set; }
    }
} 