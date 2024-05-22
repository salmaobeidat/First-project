using First_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace First_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        //added actions
        //Requirement: View all chefs with their recipes.
        [HttpGet]
        public IActionResult GetAllRecipesByCategory(int id)
        {
            var recipes = _context.Recipes
                .Include(c => c.Category)
                .Include(ch => ch.User)
                .Include(s=>s.Status)
                .Where(c => c.CategoryId == id)
                .Where(s=>s.Status.StatusId==2)
                .ToList();


           
            ViewBag.Recipes = recipes;
            return View();
        }
        [HttpPost]
        //public IActionResult GetAllRecipesByCategory(int id)
        //{


        //    return View();
        //}
        public async Task<IActionResult> Index()
        { 
            ViewBag.HomePageManagement = await _context.Homepages.FirstOrDefaultAsync();

            //viewBag to display testimonial
            var testimonial = await _context.Testimonials
                .Include(x=>x.User)
                .Include(s=>s.Status)
                .Where(x=>x.Status.StatusId==2)
                .ToListAsync();

            ViewBag.testimonials=testimonial;


            if (_context.Categories == null) return NotFound();
            var categories = await _context.Categories
                .Include(r=>r.Recipes)
                .ToListAsync();

            ViewBag.categories = categories;
            return View();
        }

        
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(Contactu contact)
        {
            if(contact.Email == null) 
            { 
             ModelState.AddModelError("", "Email is required.");
                return View(contact);
            }
            Contactu newMessage = new Contactu();

            newMessage.Email = contact.Email;
            newMessage.Subject = contact.Subject;
            newMessage.MessageContent = contact.MessageContent;

            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("ContactUs", "Home");
        }

        public async Task<IActionResult> AboutUs()
        {
            var content = await _context.Aboutus.FirstOrDefaultAsync();
            if (content == null)
            {
                return NotFound(); // Or handle the null case as required
            }
            return View(content);
        }

        //Requirement: View all chefs with their recipes  
        public async Task<IActionResult> Recipes()
        {
            ViewBag.ChefsRecipes = 
                await 
                _context.Recipes
                .Include(u => u.User)
                .ToListAsync();

            return View();
        }


        //end of the requirement
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}