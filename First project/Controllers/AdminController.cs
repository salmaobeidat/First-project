﻿using First_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace First_project.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = _context.Users
            .Include(user => user.Role);

            ViewBag.CustomerCount = userCount
            .Where(x => x.Role.RoleId == 3)
            .Count();

            ViewBag.ChefCount = userCount
           .Where(x => x.Role.RoleId == 2)
           .Count();

            ViewBag.AddedRecipes = _context.Recipes.Count();

            var adminID = HttpContext.Session.GetInt32("adminSession");
            var adminInfo = await _context.Users.Where(admin => admin.UserId == adminID).SingleOrDefaultAsync();
            return View(adminInfo);
        }

        //requirement:can view the details of registered users.
        public async Task<IActionResult> Registered()
        {
            ViewBag.RegisteredUsers
                = await
                _context.Users
                .Include(x => x.Role)
                .Include(x => x.Credential)
                .Where(x => x.Role.RoleId == 3)
                .OrderBy(x => x.UserId)
                .ToListAsync();

            return View();
        }

        public async Task<IActionResult> RegisteredChefs()
        {
            ViewBag.RegisteredChefsInfo = await _context.Users
                .Include(x => x.Credential)
                .Include(r => r.Role)
                .Where(r => r.Role.RoleId == 2)
                .ToListAsync();


            return View();
        }

        //End of requirement
        public async Task<IActionResult> IndexContactUs()
        {
            return _context.Contactus != null ?
                        View(await _context.Contactus.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Contactus'  is null.");
        }

        // GET: Categories
        public async Task<IActionResult> CategoryIndex()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        // GET: Categories/Create
        public IActionResult CategoryCreate()
        {
            return View();
        }

        // POST: Categories/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryCreate([Bind("CategoryId,CategoryName,UserId,CategoryImage")] Category category)
        {
            if (category.CategoryImage == null)
            {
                return View(category);

            }

            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + category.CategoryImage.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await category.CategoryImage.CopyToAsync(fileStream);
            }
            category.CategoryImagePath = imageName;

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("CategoryCreate", "Admin");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryEdit(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(decimal id, [Bind("CategoryId,CategoryName,UserId,CategoryImage")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (category.CategoryImage != null)
            {
                //path link
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + category.CategoryImage.FileName;
                string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

                //adding the image to the path by reading the stram of bits of the image
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await category.CategoryImage.CopyToAsync(fileStream);
                }
                category.CategoryImagePath = imageName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CategoryEdit));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> CategoryDelete(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDelete(decimal id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CategoryIndex", "Admin");
        }



        // GET: Categories/Details/5
        public async Task<IActionResult> CategoryDetails(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Aboutus
        public async Task<IActionResult> AboutUsIndex()
        {
            return _context.Aboutus != null ?
                        View(await _context.Aboutus.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Aboutus'  is null.");
        }

        // GET: Aboutus/Details/5
        public async Task<IActionResult> AboutUsDetails(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.AboutUs == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // GET: Aboutus/Create
        public IActionResult AboutUsCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AboutUsCreate([Bind("AboutUs,Header,ImageFile,AboutusContent")] Aboutu aboutu)
        {
            // Check if any AboutUs entity exists
            var exist = await _context.Aboutus.AnyAsync();

            // If any entity exists, redirect to the Index page
            if (exist)
            {
                return RedirectToAction("AboutUsIndex");
            }
            if (aboutu.ImageFile != null)
            {
                //path link
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + aboutu.ImageFile.FileName;
                string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

                //adding the image to the path by reading the stram of bits of the image
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await aboutu.ImageFile.CopyToAsync(fileStream);
                }
                aboutu.AboutusImagePath = imageName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(aboutu);
                await _context.SaveChangesAsync();
                return RedirectToAction("AboutUsIndex", "Admin");
            }
            return View(aboutu);
        }

        // GET: Aboutus/Edit/5
        public async Task<IActionResult> AboutUsEdit(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu == null)
            {
                return NotFound();
            }
            return View(aboutu);
        }



        // POST: Aboutus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AboutUsEdit(decimal id, [Bind("AboutUs,ImageFile,Header,AboutusContent")] Aboutu aboutu)
        {

            //path link
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageName = Guid.NewGuid().ToString() + aboutu.ImageFile.FileName;
            string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

            //adding the image to the path by reading the stram of bits of the image
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await aboutu.ImageFile.CopyToAsync(fileStream);
            }
            aboutu.AboutusImagePath = imageName;

            if (id != aboutu.AboutUs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuExists(aboutu.AboutUs))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("AboutUsIndex", "Admin");
            }
            return View(aboutu);
        }

        // GET: Aboutus/Delete/5
        public async Task<IActionResult> AboutUsDelete(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.AboutUs == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // POST: Aboutus/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AboutUsDelete(decimal id)
        {
            if (_context.Aboutus == null)
            {
                return Problem("Entity set 'ModelContext.Aboutus'  is null.");
            }
            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu != null)
            {
                _context.Aboutus.Remove(aboutu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("AboutUsIndex", "Admin");
        }

        //Requirement: Search for two dates on added recipes by the chef


        //Accept or reject the recipe from the chef.
        [HttpGet]
        public async Task<IActionResult> AddedRecipes()
        {
            if (_context.Recipes == null)
            {
                Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var recipes = await _context.Recipes
                .Include(u => u.User)
                .Include(u => u.Category)
                .Include(s => s.Status)
                .Where(x => x.User.RoleId == 2)
                .OrderBy(x => x.RecipeName)
                .ToListAsync();
            return View(recipes);
        }
        [HttpPost]

        //why "?" character? to prevent get the default value for date and accept null value so we can compare them
        public async Task<IActionResult> AddedRecipes(DateTime? startDate, DateTime? endDate)
        {
            if (_context.Recipes == null)
            {
                Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var recipes = await _context.Recipes
                .Include(u => u.User)
                .Include(u => u.Category)
                .Include(s => s.Status)
                .Where(x => x.User.RoleId == 2)
                .OrderBy(x => x.RecipeName)
                .ToListAsync();


            //why .Value.Date? to get date only without time
            if (startDate == null && endDate == null)
            {
                //return all data without filtration
                return View(recipes);
            }
            else if (startDate != null && endDate == null)
            {
                //return data from strating date till now
                recipes =  recipes.Where(x => x.Creationdate.Value.Date >= startDate).ToList();
                return View(recipes);
            }
            else if (startDate == null && endDate != null)
            {
                //return data from the begening till the end date
                recipes = recipes.Where(x => x.Creationdate.Value.Date <= endDate).ToList();
                return View(recipes);
            }
            else
            {
                //return data for the specific duration.
                recipes = recipes
                    .Where(
                    x =>
                    x.Creationdate.Value.Date >= startDate
                    && 
                    x.Creationdate.Value.Date <= endDate)
                    .ToList();
                return View(recipes);
            }

        }
        
        //End of requirement.
        public async Task<IActionResult> RecipeAcceptance(decimal? id)
        {

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe != null)
            {
                recipe.StatusId = 2;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AddedRecipes", "Admin");
        }


        public async Task<IActionResult> RecipeRejection(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                recipe.StatusId = 3;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AddedRecipes", "Admin");

        }


        public async Task<IActionResult> RecipePend(decimal? id)
        {

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe != null)
            {
                recipe.StatusId = 1;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AddedRecipes", "Admin");
        }

        //actions for testimonial 

        public async Task<IActionResult> AddedTestimonial()
        {
            if (_context.Testimonials == null)
            {
                Problem("Entity set 'ModelContext.Testimonial'  is null.");
            }
            var testimonials = await _context.Testimonials
                .Include(u => u.User)
                .Include(s => s.Status)
                .Where(x => x.User.RoleId == 3)
                .OrderBy(x => x.UserId)
                .ToListAsync();

            return View(testimonials);
        }


        public async Task<IActionResult> TestimonialAcceptance(decimal? id)
        {

            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);

            if (testimonial != null)
            {
                testimonial.StatusId = 2;
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AddedTestimonial", "Admin");
        }


        public async Task<IActionResult> TestimonialRejection(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                testimonial.StatusId = 3;
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AddedTestimonial", "Admin");

        }


        public async Task<IActionResult> TestimonialPend(decimal? id)
        {

            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);

            if (testimonial != null)
            {
                testimonial.StatusId = 1;
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AddedTestimonial", "Admin");
        }


        //End of actions for testimonial 

        //Requirement: Obtain monthly and annual reports showing recipes sold during a specific period.
        [HttpGet]
        public async Task<IActionResult> Order()
        {
            //To get oders 
            ViewBag.Order = await _context.Orders
                .Include(u=>u.User)
                .Include(r=>r.Recipe)
                .ToListAsync();


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Order(DateTime startDate , DateTime endDate)
        {

            if (_context.Orders == null)
            {
                Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var Order = await _context.Orders
               .Include(u => u.User)
               .Include(r => r.Recipe)
               .ToListAsync();


            //why .Value.Date? to get date only without time
            if (startDate == null && endDate == null)
            {
                //return all data without filtration
                return View(Order);
            }
            else if (startDate != null && endDate == null)
            {
                //return data from strating date till now
                Order = Order.Where(x => x.OrderDate.Value.Date >= startDate).ToList();
                return View(Order);
            }
            else if (startDate == null && endDate != null)
            {
                //return data from the begening till the end date
                Order = Order.Where(x => x.OrderDate.Value.Date <= endDate).ToList();
                return View(Order);
            }
            else
            {
                //return data for the specific duration.
                Order = Order
                    .Where(
                    x =>
                    x.OrderDate.Value.Date >= startDate
                    &&
                    x.OrderDate.Value.Date <= endDate)
                    .ToList();
                return View(Order);
            }
     
        }


        //End of requirement 
        private bool AboutuExists(decimal id)
        {
            return (_context.Aboutus?.Any(e => e.AboutUs == id)).GetValueOrDefault();
        }
        private bool CategoryExists(decimal id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
