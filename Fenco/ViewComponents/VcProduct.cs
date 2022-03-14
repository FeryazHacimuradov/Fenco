using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fenco.ViewModels;
using Fenco.Data;
using Fenco.Models;
using Microsoft.EntityFrameworkCore;

namespace Fenco.ViewComponents
{
    public class VcProduct: ViewComponent
    {
        private readonly AppDbContext _context;

        public VcProduct(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = await _context.Products
                                       .Include(cp => cp.ColorToProducts).ThenInclude(pi => pi.ProductImages)
                                       .Include(cp => cp.ColorToProducts).ThenInclude(sc => sc.SizeColorToProducts)
                                       .OrderByDescending(o => o.CreatedDate)
                                       .Take(8).ToListAsync();

            return View(products);
        }
    }
}
