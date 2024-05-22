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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Status).Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
           
            var testimonial = await _context.Testimonials
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestimonialId == id);



            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {

            var userId= HttpContext.Session.GetInt32("customerSession");
            if(userId==null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            //to get the id by session.
            testimonial.UserId = HttpContext.Session.GetInt32("customerSession");
            
            testimonial.CreationDate = DateTime.Now; // Set the current date and time

            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                //TempData["Success"] = "Thank you for your feedback!";
                return RedirectToAction("Create","Testimonials");
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", testimonial.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", testimonial.UserId);
            return View(testimonial);
        }

        

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestimonialId == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AddedTestimonial","Admin");
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.TestimonialId == id)).GetValueOrDefault();
        }
    }
}
