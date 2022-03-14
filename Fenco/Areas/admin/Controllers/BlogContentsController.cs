using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fenco.Data;
using Fenco.Models;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    public class BlogContentsController : Controller
    {
        private readonly AppDbContext _context;

        public BlogContentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/BlogContents
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogContents.ToListAsync());
        }

        // GET: admin/BlogContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogContent == null)
            {
                return NotFound();
            }

            return View(blogContent);
        }

        // GET: admin/BlogContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/BlogContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] BlogContent blogContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogContent);
        }

        // GET: admin/BlogContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents.FindAsync(id);
            if (blogContent == null)
            {
                return NotFound();
            }
            return View(blogContent);
        }

        // POST: admin/BlogContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] BlogContent blogContent)
        {
            if (id != blogContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogContentExists(blogContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogContent);
        }

        // GET: admin/BlogContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogContent == null)
            {
                return NotFound();
            }

            return View(blogContent);
        }

        // POST: admin/BlogContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogContent = await _context.BlogContents.FindAsync(id);
            _context.BlogContents.Remove(blogContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogContentExists(int id)
        {
            return _context.BlogContents.Any(e => e.Id == id);
        }
    }
}
