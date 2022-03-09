using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fenco.Data;
using Fenco.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        // GET: admin/Sliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: admin/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                if (slider.BgImageFile.ContentType == "image/jpeg" || slider.BgImageFile.ContentType == "image/png")
                {
                    if (slider.BgImageFile.Length <= 3145728)
                    {
                        string fileName = Guid.NewGuid() + "-" + slider.BgImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            slider.BgImageFile.CopyTo(stream);
                        }

                        slider.BgImage = fileName;
                        _context.Sliders.Add(slider);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Upload max 3Mb image file");
                        return View(slider);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Uploadc only Image File!");
                    return View(slider);
                }
            }
            else
            {
                return View(slider);
            }
        }

        // GET: admin/Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                if (slider.BgImage != null)
                {
                    if (slider.BgImageFile.ContentType == "image/jpeg" || slider.BgImageFile.ContentType == "image/png")
                    {
                        if (slider.BgImageFile.Length <= 3145728)
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", slider.BgImage);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                            string fileName = Guid.NewGuid() + "-" + slider.BgImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                slider.BgImageFile.CopyTo(stream);
                            }

                            slider.BgImage = fileName;
                            _context.Sliders.Update(slider);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Upload max 3Mb image file");
                            return View(slider);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Uploadc only Image File!");
                        return View(slider);
                    }
                }
                else
                {
                    _context.Sliders.Update(slider);
                    _context.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            else
            {
                return View(slider);

            }
        }

        // GET: admin/Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", slider.BgImage);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
