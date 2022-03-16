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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            VmHome model = new VmHome();
            model.Setting = _context.Settings.FirstOrDefault();
            model.Socials = _context.Socials.ToList();
            model.Services = _context.Services.ToList();
            model.Categories = _context.Categories.Include(b => b.Blogs).ToList();
            model.Blogs = _context.Blogs.Include(i => i.BlogImage).Include(c => c.BlogContent).ToList();

            return View(model);
        }
        public IActionResult Details(int id)
        {
            VmHome model = new VmHome();
            model.Blog = _context.Blogs.Include(i => i.BlogImage).Include(c => c.BlogContent).FirstOrDefault(b => b.Id == id);

            return View(model);
        }
    }
}
