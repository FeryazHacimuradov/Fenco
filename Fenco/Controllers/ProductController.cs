using Fenco.Data;
using Fenco.Models;
using Fenco.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(VmProduct model)
        {
            model.Setting = _context.Settings.FirstOrDefault();
            model.Socials = _context.Socials.ToList();
            model.Services = _context.Services.ToList();

            List<Product> products = _context.Products
                                                      .Include(cp => cp.ColorToProducts).ThenInclude(pi => pi.ProductImages)
                                                      .Include(cp => cp.ColorToProducts).ThenInclude(sc => sc.SizeColorToProducts)
                                                      .Where(p => (model.searchData != null ? p.Name.Contains(model.searchData) : true) &&
                                                                  (model.prdId != null ? p.CategoryId == model.prdId : true) &&
                                                                  (model.MinPrice != null ? p.ColorToProducts.FirstOrDefault().SizeColorToProducts.FirstOrDefault().Price >= model.MinPrice : true) &&
                                                                  (model.MaxPrice != null ? p.ColorToProducts.FirstOrDefault().SizeColorToProducts.FirstOrDefault().Price <= model.MaxPrice : true))
                                                      .ToList();

            model.PageCount = (int)Math.Ceiling(products.Count / model.ItemCount);
            model.Products = products.Skip((model.Page - 1) * (int)model.ItemCount).Take((int)model.ItemCount).ToList();
            model.ProductCategories = _context.ProductCategories.ToList();

            return View(model); 
        }
    }
}
