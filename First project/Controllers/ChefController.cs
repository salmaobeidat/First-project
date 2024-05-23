using First_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace First_project.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;

        public ChefController(ModelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //open session with chef
            var chefId =  HttpContext.Session.GetInt32("chefSession");
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
    }
}
