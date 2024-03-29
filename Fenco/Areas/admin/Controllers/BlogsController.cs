﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fenco.Data;
using Fenco.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;

        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/Blogs

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Blogs.Include(b => b.BlogContent).Include(b => b.BlogImage).Include(b => b.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogContent)
                .Include(b => b.BlogImage)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id");
            ViewData["BlogImageId"] = new SelectList(_context.BlogImages, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CustomUserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,BlogImageId,BlogContentId,CategoryId,CreatedDate,CustomUserId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blog.BlogContentId);
            ViewData["BlogImageId"] = new SelectList(_context.BlogImages, "Id", "Id", blog.BlogImageId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blog.CategoryId);
            ViewData["CustomUserId"] = new SelectList(_context.Users, "Id", "Name", blog.CustomUserId);
            return View(blog);
        }

        // GET: admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blog.BlogContentId);
            ViewData["BlogImageId"] = new SelectList(_context.BlogImages, "Id", "Id", blog.BlogImageId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blog.CategoryId);
            ViewData["CustomUserId"] = new SelectList(_context.Users, "Id", "Name", blog.CustomUserId);
            return View(blog);
        }

        // POST: admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,BlogImageId,BlogContentId,CategoryId,CreatedDate,CustomUserId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blog.BlogContentId);
            ViewData["BlogImageId"] = new SelectList(_context.BlogImages, "Id", "Id", blog.BlogImageId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blog.CategoryId);
            ViewData["CustomUserId"] = new SelectList(_context.Users, "Id", "Name", blog.CustomUserId);
            return View(blog);
        }

        // GET: admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogContent)
                .Include(b => b.BlogImage)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
