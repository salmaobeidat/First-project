using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_project.Models;
using Microsoft.AspNetCore.Hosting;

namespace First_project.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecipesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var chefId = HttpContext.Session.GetInt32("chefSession");
            if (chefId == null)
            {
                return RedirectToAction("Login", "Auth");
            }


            var modelContext = _context.Recipes.Include(r => r.Category)
                .Include(r => r.Status).Include(r => r.User)
                .Where(r=>r.User.UserId==chefId);
            return View(await modelContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var chefId = HttpContext.Session.GetInt32("chefSession");
            if ( chefId ==null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create(decimal? id)
        {
            var chefId = HttpContext.Session.GetInt32("chefSession");
            if ( chefId==null)
            {
                return RedirectToAction("Login", "Auth");
            }


            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            recipe.UserId = HttpContext.Session.GetInt32("chefSession");
            recipe.RepcipePdfPath = "_";

            if(recipe.RecipeImage== null)
            {
                return View(recipe);

            }
            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + recipe.RecipeImage.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/RecipesImages/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await recipe.RecipeImage.CopyToAsync(fileStream);
            }
            recipe.RecipeImagePath = imageName;

            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", recipe.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var chefId = HttpContext.Session.GetInt32("chefSession");
            if (chefId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", recipe.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (recipe.RecipeImage == null)
            {
                return View(recipe);

            }
            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + recipe.RecipeImage.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/RecipesImages/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await recipe.RecipeImage.CopyToAsync(fileStream);
            }
            recipe.RecipeImagePath = imageName;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recipe.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", recipe.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
