using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_project.Models;

namespace First_project.Controllers
{
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomepagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Homepages
        public async Task<IActionResult> Index()
        {
              return _context.Homepages != null ? 
                          View(await _context.Homepages.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homepages'  is null.");
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Homepage homepage)
        {
            // Check if any AboutUs entity exists
            var exist = await _context.Homepages.AnyAsync();

            // If any entity exists, redirect to the Index page
            if (exist)
            {
                return RedirectToAction("Index");
            }

            if (homepage.BackgroundImage == null)
            {
                return View(homepage);

            }

            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + homepage.BackgroundImage.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await homepage.BackgroundImage.CopyToAsync(fileStream);
            }
            homepage.BackgroundImagePath = imageName;

            if (ModelState.IsValid)
            {
                _context.Add(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homepage);
        }

        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage == null)
            {
                return NotFound();
            }
            return View(homepage);
        }

        // POST: Homepages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id,Homepage homepage)
        {


            if (id != homepage.HomeId)
            {
                return NotFound();
            }
            if (homepage.BackgroundImage == null)
            {
                return View(homepage);

            }

            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + homepage.BackgroundImage.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await homepage.BackgroundImage.CopyToAsync(fileStream);
            }
            homepage.BackgroundImagePath = imageName;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homepage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepageExists(homepage.HomeId))
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
            return View(homepage);
        }

        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepages == null)
            {
                return Problem("Entity set 'ModelContext.Homepages'  is null.");
            }
            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage != null)
            {
                _context.Homepages.Remove(homepage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
          return (_context.Homepages?.Any(e => e.HomeId == id)).GetValueOrDefault();
        }
    }
}
