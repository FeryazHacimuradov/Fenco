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

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    public class BlogImagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogImagesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/BlogImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogImages.ToListAsync());
        }

        // GET: admin/BlogImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogImage = await _context.BlogImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogImage == null)
            {
                return NotFound();
            }

            return View(blogImage);
        }

        // GET: admin/BlogImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/BlogImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogImage blogImage)
        {
            if (ModelState.IsValid)
            {
                if (blogImage.ImageFile.ContentType == "image/jpeg" || blogImage.ImageFile.ContentType == "image/png")
                {
                    if (blogImage.ImageFile.Length <= 3145728)
                    {
                        string fileName = Guid.NewGuid() + "-" + blogImage.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            blogImage.ImageFile.CopyTo(stream);
                        }

                        blogImage.Image = fileName;
                        _context.BlogImages.Add(blogImage);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Upload max 3Mb image file");
                        return View(blogImage);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Uploadc only Image File!");
                    return View(blogImage);
                }
            }
            else
            {
                return View(blogImage);
            }
        }

        // GET: admin/BlogImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogImage = await _context.BlogImages.FindAsync(id);
            if (blogImage == null)
            {
                return NotFound();
            }
            return View(blogImage);
        }

        // POST: admin/BlogImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogImage blogImage)
        {
            if (ModelState.IsValid)
            {
                if (blogImage.Image != null)
                {
                    if (blogImage.ImageFile.ContentType == "image/jpeg" || blogImage.ImageFile.ContentType == "image/png")
                    {
                        if (blogImage.ImageFile.Length <= 3145728)
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", blogImage.Image);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                            string fileName = Guid.NewGuid() + "-" + blogImage.ImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                blogImage.ImageFile.CopyTo(stream);
                            }

                            blogImage.Image = fileName;
                            _context.BlogImages.Update(blogImage);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Upload max 3Mb image file");
                            return View(blogImage);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Uploadc only Image File!");
                        return View(blogImage);
                    }
                }
                else
                {
                    _context.BlogImages.Update(blogImage);
                    _context.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            else
            {
                return View(blogImage);
            }
        }

        // GET: admin/BlogImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogImage = await _context.BlogImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogImage == null)
            {
                return NotFound();
            }

            return View(blogImage);
        }

        // POST: admin/BlogImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogImage = await _context.BlogImages.FindAsync(id);
            _context.BlogImages.Remove(blogImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogImageExists(int id)
        {
            return _context.BlogImages.Any(e => e.Id == id);
        }
    }
}
