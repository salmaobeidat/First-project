using First_project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Net.Mail;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace First_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        //Requirement: View all chefs with their recipes 
        [HttpGet]
        public async Task<IActionResult> AllChefsRecipes()
        {
            var recipeByChef = await _context.Recipes
                .Include(u => u.User)
                .Include(r=>r.Status)
                .Include(c=>c.Category)
                .Where(u=>u.Status.StatusId==2)
                .ToListAsync();
            

            return View(recipeByChef);
        }

        //End of requirement

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
             var ChefsRecipes = 
                await 
                _context.Recipes
                .Include(u => u.User)
                .Where(x=>x.Status.StatusId==2)
                .ToListAsync();

            return View(ChefsRecipes);
        }
        //end of the requirement

        //Requirement : View all recipes
        [HttpPost]
        public async Task<IActionResult> Recipes(string? recipeName)
        {
            // Build the query to include User and Status and filter by StatusId
            var recipesQuery = _context.Recipes
                                       .Include(u => u.User)
                                       .Include(s => s.Status)
                                       .Where(x => x.Status.StatusId == 2)
                                       .AsQueryable();

            // If a recipe name is provided, filter by it
            if (!string.IsNullOrEmpty(recipeName))
            {
                var lowerCaseRecipeName = recipeName.ToLower();
                recipesQuery = recipesQuery.Where(r => r.RecipeName.ToLower().Contains(lowerCaseRecipeName));
            }
            // Execute the query and get the result
            var ChefsRecipes = await recipesQuery.ToListAsync();

            // Pass the result to the view
            return View(ChefsRecipes);
        }
        //End of requirement 


        //Requirement: View profile --> For Customer
        [HttpGet]
        public async Task<IActionResult> ViewProfile(decimal? id = null)
        {
            var sessionId = HttpContext.Session.GetInt32("customerSession");

            if (id == null || _context.Users == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Retrieve the adminSession from session and cast it to decimal
            //var sessionId = HttpContext.Session.GetInt32("adminSession");
            if (sessionId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            id = (decimal)sessionId;

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
            var session = HttpContext.Session.GetInt32("customerSession");
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Find if the user is exist or not
                    var userInfo = await _context.Users.Where(u => u.UserId == session).SingleOrDefaultAsync();

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
                return RedirectToAction("UpdateProfile", "Home");
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return RedirectToAction("UpdateProfile", "Home");
        }

        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        //End of requirement


        //Requirment: Buy recipes
        [HttpGet]
        public async Task<IActionResult> Purchase()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Purchase(decimal? recipeId, Visainfo visainfo)
        {
            // if user is not loged in it will redirect him to login.
            var userId = HttpContext.Session.GetInt32("customerSession");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            //To get recipe price --> it returns one recipe record
            var recipeInfo= await _context.Recipes.Where(r=>r.RecipeId == recipeId).FirstOrDefaultAsync();

            //to compare entered data with ones in database
            // && because all data should be Identical.
            VisaChecher visaCHecker = new VisaChecher();

            if(visainfo.CardHolderName == visaCHecker.CardHolderName
                &&
                visainfo.CardNumber == visaCHecker.CardNumber
                &&
                visaCHecker.Cvc == visainfo.Cvc
                ) 
                {
                if(visaCHecker.Balance>=recipeInfo.Price)
                {
                    visaCHecker.Balance-=recipeInfo.Price;

                    return RedirectToAction("Greetful", "Home");
                }
                else
                {
                    return RedirectToAction("NoEnoughBalance", "Home");
                }
                }

            else
            {
                return View(visainfo);
            }
           
        }


        //No sufficient balance page

        public async Task<IActionResult> NoEnoughBalance()
        {
            return View();
        
        }
        //End of requirement

        //Greetful action to be greetful to user

        public async Task<IActionResult> Greetful()
        {
            return View();
        }
        //End of greetfull
        //Requirement: Send email
        public async Task<IActionResult> SendEmail()
        {
            //Take email from session.
            var sessionId = HttpContext.Session.GetInt32("customerSession");
            var customerInfo = 
                 await _context.Users
                .Include(c=>c.Credential)
                .Where(u=>u.UserId==sessionId)
                .SingleOrDefaultAsync();

            //all email container to collect all data and send them once.
            MailMessage mail = new MailMessage();
            //Simple mail transfer protocol
            //Domain of Website's owner mail
            SmtpClient smtpServer = new SmtpClient("smtp.ethereal.email");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("millie.breitenberg@ethereal.email", "KwVZqGEjAcz5KB2jhN");
            //To use secure data layer
            smtpServer.EnableSsl = true;


            //Email should be real to send email
            mail.From = new MailAddress("millie.breitenberg@ethereal.email");
            //Why Add? because we can send the email to many people
            mail.To.Add($"{customerInfo?.Credential?.Email}");
            mail.Subject = "Purchase Completed.";
            mail.Body = "Thank you for using Recipe blog website, we are honored to have you as client.";

            //PDF link path
            Attachment attachment = new Attachment("");

            //we use Add when there is an ability to send more than part of data
            mail.Attachments.Add(attachment);

            smtpServer.Send(mail);

            return RedirectToAction("Greetful","Home");
        }

        //End of requirement
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