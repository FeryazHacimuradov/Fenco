using Fenco.Data;
using Fenco.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;

        public PortfolioController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            VmHome model = new VmHome();
            model.Categories = _context.Categories.Include(b => b.Blogs).ToList();
            model.Portfolios = _context.Portfolios.Include(i => i.PortfolioImage).Where(p => (model.prId != null ? p.CategoryId == model.prId : true)).ToList();
            model.Tags = _context.Tags.ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            VmHome model = new VmHome();
            model.Portfolio = _context.Portfolios.Include(i => i.PortfolioImage).Include(tg => tg.TagToPortfolios).ThenInclude(t => t.Tag).FirstOrDefault(p => p.Id == id);

            return View(model);
        }
    }
}
