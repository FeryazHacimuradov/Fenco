using Fenco.Data;
using Fenco.Models;
using Fenco.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products
                                        .Include(cp=>cp.ColorToProducts).ThenInclude(pi=>pi.ProductImages)
                                        .Include(cp => cp.ColorToProducts).ThenInclude(sc => sc.SizeColorToProducts).ThenInclude(s=>s.Size)
                                        .Include(c=>c.ProductCategory)
                                        .Include(b=>b.Brand).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.ProductCategories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                VmPrdAll vmPrdAll = new VmPrdAll();
                vmPrdAll.Product = model;
                string prdModel = JsonConvert.SerializeObject(vmPrdAll);
                HttpContext.Session.SetString("Product", prdModel);
                return RedirectToAction("CreateColorToProduct");
            }

            ViewBag.Categories = _context.ProductCategories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View(model);
        }


        public IActionResult CreateColorToProduct()
        {

            ViewBag.Colors = _context.Colors.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateColorToProduct(List<VmColorImage> model)
        {
            string prdModelString = HttpContext.Session.GetString("Product");
            VmPrdAll prdModel = JsonConvert.DeserializeObject<VmPrdAll>(prdModelString);

            foreach (var item in model)
            {
                foreach (var image in item.Image)
                {
                    string s = null;
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        s = Convert.ToBase64String(fileBytes);
                    }

                    item.ImageBase64.Add(s);
                }


                item.Image = null;
            }

            prdModel.ColorImages = model;
            HttpContext.Session.SetString("Product", JsonConvert.SerializeObject(prdModel));
            return RedirectToAction("CreateSizeToColorToProduct");
        }


        public IActionResult CreateSizeToColorToProduct()
        {
            string prdModelString = HttpContext.Session.GetString("Product");
            VmPrdAll prdModel = JsonConvert.DeserializeObject<VmPrdAll>(prdModelString);

            List<Color> colors = new List<Color>();
            foreach (var item in prdModel.ColorImages)
            {
                colors.Add(_context.Colors.FirstOrDefault(c => c.Id == item.ColorId));
            }
            //ViewBag.Colors = _context.Colors.Where(c => prdModel.ColorImages.FirstOrDefault(ci => ci.ColorId == c.Id)!=null).ToList();
            ViewBag.Colors = colors;
            ViewBag.Sizes = _context.Sizes.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateSizeToColorToProduct(List<VmSizeToColor> model)
        {
            string prdModelString = HttpContext.Session.GetString("Product");
            VmPrdAll prdModel = JsonConvert.DeserializeObject<VmPrdAll>(prdModelString);

            //Step 1
            Product product = new Product()
            {
                Name = prdModel.Product.Name,
                About = prdModel.Product.About,
                Description = prdModel.Product.Description,
                CategoryId = prdModel.Product.CategoryId,
                BrandId = prdModel.Product.BrandId
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            //Step 2
            foreach (var item in prdModel.ColorImages)
            {
                ColorToProduct colorToProduct = new ColorToProduct()
                {
                    ProductId = product.Id,
                    ColorId = item.ColorId
                };
                _context.ColorToProducts.Add(colorToProduct);
                _context.SaveChanges();

                foreach (var image in item.ImageBase64)
                {
                    byte[] bytes = Convert.FromBase64String(image);
                    MemoryStream stream = new MemoryStream(bytes);

                    IFormFile file = new FormFile(stream, 0, bytes.Length, "image1", "image1.png");

                    string filename = Guid.NewGuid() + "-" + file.FileName;
                    string filePath = Path.Combine("wwwroot", "Uploads", filename);

                    using (var stream2 = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream2);
                    }

                    ProductImage productImage = new ProductImage()
                    {
                        Image = filename,
                        ColorToProductId = colorToProduct.Id
                    };
                    _context.ProductImages.Add(productImage);
                    _context.SaveChanges();
                }

                //Step 3
                foreach (var sizeColor in model)
                {
                    if (sizeColor.ColorId == colorToProduct.ColorId)
                    {
                        SizeColorToProduct sizeColorToProduct = new SizeColorToProduct();
                        sizeColorToProduct.ColorToProductId = colorToProduct.Id;
                        sizeColorToProduct.SizeId = sizeColor.SizeId;
                        sizeColorToProduct.Price = sizeColor.Price;
                        sizeColorToProduct.Quantity = sizeColor.Quantity;

                        _context.SizeColorToProducts.Add(sizeColorToProduct);
                        _context.SaveChanges();
                    }
                }
            }

            HttpContext.Session.Remove("Product");
            return RedirectToAction("index");
        }

    }
}
