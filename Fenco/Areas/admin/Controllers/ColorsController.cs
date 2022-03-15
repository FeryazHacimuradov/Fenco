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
    public class ColorsController : Controller
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/Colors
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Colors.Include(c => c.ParentColor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: admin/Colors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
                .Include(c => c.ParentColor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // GET: admin/Colors/Create
        public IActionResult Create()
        {
            ViewData["ParentColorId"] = new SelectList(_context.Colors, "Id", "Name");
            return View();
        }

        // POST: admin/Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentColorId")] Color color)
        {
            if (ModelState.IsValid)
            {
                _context.Add(color);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentColorId"] = new SelectList(_context.Colors, "Id", "Name", color.ParentColorId);
            return View(color);
        }

        // GET: admin/Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            ViewData["ParentColorId"] = new SelectList(_context.Colors, "Id", "Name", color.ParentColorId);
            return View(color);
        }

        // POST: admin/Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentColorId")] Color color)
        {
            if (id != color.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(color);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.Id))
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
            ViewData["ParentColorId"] = new SelectList(_context.Colors, "Id", "Name", color.ParentColorId);
            return View(color);
        }

        // GET: admin/Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
                .Include(c => c.ParentColor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // POST: admin/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorExists(int id)
        {
            return _context.Colors.Any(e => e.Id == id);
        }
    }
}
