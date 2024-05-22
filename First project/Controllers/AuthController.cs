using First_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace First_project.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;

        public AuthController(ModelContext context) 
        { 
            _context = context;
        }
        public IActionResult RegisterOption()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CustomerRegister()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChefRegister()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ChefRegister(Register register)
        {
            var checkEmail = await _EmailChecker(register.Email);
            if (checkEmail == true)
            {
                ModelState.AddModelError("", "Email already exists.");
                //to return the user to the same page with entereed informations.
                return View(register);
            }

            User user = new User();
            user.UserName = register.UserName;
            user.Gender = register.Gender;
            user.Birthdate = register.Birthdate;
            user.Specialization= register.Specialization;
            user.RoleId = 2;
            
           await _context.AddAsync(user);
           await _context.SaveChangesAsync();

            Credential credential = new Credential();
            credential.Email = register.Email;
            credential.Password = register.Password;
            credential.UserId=user.UserId;

            await _context.AddAsync(credential);
            await _context.SaveChangesAsync();


            return RedirectToAction("Login", "Auth");

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> CustomerRegister(Register register)
        {
            var checkEmail =await _EmailChecker(register.Email);
            if (checkEmail == true)
            {
                ModelState.AddModelError("", "Email already exists.");
                //to return the user to the same page with entereed informations.
                return View(register);
            }

            User user = new User();
            user.UserName = register.UserName;
            user.RoleId = 3;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            Credential credential = new Credential();
            credential.Email = register.Email.ToLower().Trim();
            credential.Password = register.Password;
            credential.UserId = user.UserId;

            await _context.AddAsync(credential);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

            //return View();
        }

        public async Task<IActionResult>Login(Credential credential)
        {
            if(!ModelState.IsValid)
            {
                return View(credential);
            }

            var user = await _context.Credentials
                .Include(userInfo => userInfo.User)
                .SingleOrDefaultAsync(userInfo => userInfo.Email == credential.Email && userInfo.Password == credential.Password);


            //to get one record or null

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password.");
                return View(credential); //to not fill informations again.
            }
            var roleId = user.User?.RoleId;

            if (user != null)
            {
                
                switch(roleId)
                {
                    case 1:
                        //to store ID in the session ,so that you can use it in any part in the project.
                        HttpContext.Session.SetInt32("adminSession", (int)user.UserId);
                        //action,controller
                        return RedirectToAction("Index","Admin");
                    case 2:
                        HttpContext.Session.SetInt32("chefSession",(int)user.UserId);
                        return RedirectToAction("Index", "Chef");
                    case 3:
                        HttpContext.Session.SetInt32("customerSession", (int)user.UserId);
                        return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }



        private async Task<bool> _EmailChecker(string email)
        {
            var emailExistance = await _context.Credentials.SingleOrDefaultAsync(check => check.Email == email);

            if (emailExistance!=null) {
                return true;
            }
            return false;

        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return View(nameof(Login));
        }
    }
}
