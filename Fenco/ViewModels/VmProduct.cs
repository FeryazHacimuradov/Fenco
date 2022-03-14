using Fenco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.ViewModels
{
    public class VmProduct:VmLayout
    {
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<SizeColorToProduct> SizeColorToProducts { get; set; }
        public List<ColorToProduct> ColorToProducts { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
        public int PageCount { get; set; }
        public double ItemCount { get; set; } = 12;
        public int Page { get; set; } = 1;
        public int? prdId { get; set; }
        public string searchData { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
