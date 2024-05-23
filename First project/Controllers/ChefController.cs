using First_project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace First_project.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChefController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            //open session with chef
            var chefId =  HttpContext.Session.GetInt32("chefSession");

            if(chefId==null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var chefInfo = await _context.Users.Where(x => x.UserId == chefId).SingleOrDefaultAsync();

            ViewBag.categories =await _context.Categories.ToListAsync();
            ViewBag.recipes =await _context.Recipes.ToListAsync();

            return View(chefInfo);
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


        //view all chefs with their recipe
        //Requirement: Search for the name of the receipe
        [HttpGet]
        public async Task<IActionResult> RecipeByChef()
        {
                 var ChefsRecipes =
                 await _context.Recipes
                .Include(u => u.User)
                .Include(c => c.Category)
                .Include(s => s.Status)
                .Include(r => r.User.Role)
                .Where(s => s.Status.StatusId == 2)
                .ToListAsync();

            return View(ChefsRecipes);
        }

        [HttpPost]
        public async Task<IActionResult> RecipeByChef(string? recipeName)
        {
            // Start with the base query
            var query = _context.Recipes
                .Include(u => u.User)
                .Include(c => c.Category)
                .Include(s => s.Status)
                .Include(r => r.User.Role)
                .Where(s => s.Status.StatusId == 2);

            // Apply filtering if a recipe name is provided
            if (!string.IsNullOrEmpty(recipeName))
            {
                var lowerCaseRecipeName = recipeName.ToLower();
                query = query.Where(r => r.RecipeName.ToLower().Contains(lowerCaseRecipeName));
            }

            // Execute the query and get the result
            var ChefsRecipes = await query.ToListAsync();

            return View(ChefsRecipes);
        }

        //End of requirement

        //Requirement: View profile --> For Customer
        [HttpGet]
        public async Task<IActionResult> ViewProfile(decimal? id = null)
        {

            if (id == null || _context.Users == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //End of requirement

        //Requirement: Update profile --> for customer
        [HttpGet]
        public async Task<IActionResult> UpdateProfile(decimal? id)
        {

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(decimal id, User user)
        {
            var session = HttpContext.Session.GetInt32("chefSession");
            if (id != user.UserId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    //Find if the user is exist or not
                    var userInfo = await _context.Users
                        .Where(u => u.UserId == id).SingleOrDefaultAsync();

                    //upload image 
                    //path link
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + user.ProfileImage.FileName;
                    string fullPath = Path.Combine(wwwRootPath + "/images/", imageName);

                    //adding the image to the path by reading the stram of bits of the image
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await user.ProfileImage.CopyToAsync(fileStream);
                    }

                    //update values based on the recieving object from user.
                    userInfo.UserName = user.UserName;

                    userInfo.Gender = user.Gender;

                    userInfo.Birthdate = user.Birthdate.Value.Date;
                    userInfo.Profileimagepath = imageName;
                    userInfo.RoleId = 2;
                    userInfo.Specialization = user.Specialization;
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //To pass the User id to view.
                return RedirectToAction(nameof(ViewProfile), new { id = user.UserId });
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
             return View(user);
        }

        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        //End of requirement
    }
}
