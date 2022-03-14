using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fenco.Data;
using Fenco.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class PortfolioImagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PortfolioImagesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/PortfolioImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.PortfolioImages.ToListAsync());
        }

        // GET: admin/PortfolioImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioImage = await _context.PortfolioImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolioImage == null)
            {
                return NotFound();
            }

            return View(portfolioImage);
        }

        // GET: admin/PortfolioImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/PortfolioImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PortfolioImage portfolioImage)
        {
            if (ModelState.IsValid)
            {
                if (portfolioImage.ImageFile.ContentType == "image/jpeg" || portfolioImage.ImageFile.ContentType == "image/png")
                {
                    if (portfolioImage.ImageFile.Length <= 3145728)
                    {
                        string fileName = Guid.NewGuid() + "-" + portfolioImage.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            portfolioImage.ImageFile.CopyTo(stream);
                        }

                        portfolioImage.Image = fileName;
                        _context.PortfolioImages.Add(portfolioImage);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Upload max 3Mb image file");
                        return View(portfolioImage);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Uploadc only Image File!");
                    return View(portfolioImage);
                }
            }
            else
            {
                return View(portfolioImage);
            }
        }

        // GET: admin/PortfolioImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioImage = await _context.PortfolioImages.FindAsync(id);
            if (portfolioImage == null)
            {
                return NotFound();
            }
            return View(portfolioImage);
        }

        // POST: admin/PortfolioImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PortfolioImage portfolioImage)
        {
            if (ModelState.IsValid)
            {
                if (portfolioImage.Image != null)
                {
                    if (portfolioImage.ImageFile.ContentType == "image/jpeg" || portfolioImage.ImageFile.ContentType == "image/png")
                    {
                        if (portfolioImage.ImageFile.Length <= 3145728)
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", portfolioImage.Image);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                            string fileName = Guid.NewGuid() + "-" + portfolioImage.ImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                portfolioImage.ImageFile.CopyTo(stream);
                            }

                            portfolioImage.Image = fileName;
                            _context.PortfolioImages.Update(portfolioImage);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Upload max 3Mb image file");
                            return View(portfolioImage);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Uploadc only Image File!");
                        return View(portfolioImage);
                    }
                }
                else
                {
                    _context.PortfolioImages.Update(portfolioImage);
                    _context.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            else
            {
                return View(portfolioImage);
            }
        }

        // GET: admin/PortfolioImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioImage = await _context.PortfolioImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolioImage == null)
            {
                return NotFound();
            }

            return View(portfolioImage);
        }

        // POST: admin/PortfolioImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolioImage = await _context.PortfolioImages.FindAsync(id);
            _context.PortfolioImages.Remove(portfolioImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioImageExists(int id)
        {
            return _context.PortfolioImages.Any(e => e.Id == id);
        }
    }
}
