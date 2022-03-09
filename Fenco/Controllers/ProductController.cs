using Fenco.Data;
using Fenco.Models;
using Fenco.ViewModels;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = _context.Products
                                               .Include(c => c.ColorToProducts).ThenInclude(pi => pi.ProductImages)
                                               .Include(c => c.ColorToProducts).ThenInclude(sp => sp.SizeColorToProducts).ThenInclude(s => s.Size)
                                               .Include(c => c.ColorToProducts).ThenInclude(co => co.Color)
                                               .Include(ca => ca.ProductCategory)
                                               .FirstOrDefault(p => p.Id == id);

            return View(product);
        }

        public IActionResult AddToCart(int sizeColorproductId)
        {
            string oldCart = Request.Cookies["cart"];
            string newCart = "";

            if (string.IsNullOrEmpty(oldCart))
            {
                newCart = sizeColorproductId + "";
            }
            else
            {
                List<string> oldCartList = oldCart.Split("-").ToList();

                if (oldCartList.Any(i=>i==sizeColorproductId.ToString()))
                {
                    oldCartList.Remove(sizeColorproductId.ToString());
                }
                else
                {
                    oldCartList.Add(sizeColorproductId.ToString());
                }

                newCart = string.Join("-", oldCartList);
            }

            Response.Cookies.Append("cart", newCart);
            return RedirectToAction("index");
        }

        public IActionResult Cart()
        {
            VmProduct model = new VmProduct();
            model.Setting = _context.Settings.FirstOrDefault();
            model.Socials = _context.Socials.ToList();
            model.Services = _context.Services.ToList();

            string cart = Request.Cookies["cart"];
            List<SizeColorToProduct> sizeColorToProducts = new List<SizeColorToProduct>();
            if (!string.IsNullOrEmpty(cart))
            {
                List<string> cartList = cart.Split("-").ToList();

                sizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.ProductImages)
                                                                  .Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                  .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
            }

            return View(sizeColorToProducts);
        }
    }
}
