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
    public class PortfoliosController : Controller
    {
        private readonly AppDbContext _context;

        public PortfoliosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/Portfolios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Portfolios.Include(p => p.Category).Include(p => p.PortfolioImage);
            return View(await appDbContext.ToListAsync());
        }

        // GET: admin/Portfolios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .Include(p => p.Category)
                .Include(p => p.PortfolioImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // GET: admin/Portfolios/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["PortfolioImageId"] = new SelectList(_context.PortfolioImages, "Id", "Id");
            return View();
        }

        // POST: admin/Portfolios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PortfolioImageId,Content,CategoryId,CreatedDate")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            ViewData["PortfolioImageId"] = new SelectList(_context.PortfolioImages, "Id", "Id", portfolio.PortfolioImageId);
            return View(portfolio);
        }

        // GET: admin/Portfolios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            ViewData["PortfolioImageId"] = new SelectList(_context.PortfolioImages, "Id", "Id", portfolio.PortfolioImageId);
            return View(portfolio);
        }

        // POST: admin/Portfolios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PortfolioImageId,Content,CategoryId,CreatedDate")] Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioExists(portfolio.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", portfolio.CategoryId);
            ViewData["PortfolioImageId"] = new SelectList(_context.PortfolioImages, "Id", "Id", portfolio.PortfolioImageId);
            return View(portfolio);
        }

        // GET: admin/Portfolios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .Include(p => p.Category)
                .Include(p => p.PortfolioImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // POST: admin/Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioExists(int id)
        {
            return _context.Portfolios.Any(e => e.Id == id);
        }
    }
}
