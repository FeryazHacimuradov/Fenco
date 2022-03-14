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
    public class VcBlog: ViewComponent
    {
        private readonly AppDbContext _context;

        public VcBlog(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Blog> blogs = await _context.Blogs
                                                   .Include(i => i.BlogImage)
                                                   .Include(c => c.BlogContent).OrderByDescending(o => o.CreatedDate)
                                                   .Take(3).ToListAsync();

            return View(blogs);
        }
    }
}
