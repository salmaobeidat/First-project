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
    public class VisaChechersController : Controller
    {
        private readonly ModelContext _context;

        public VisaChechersController(ModelContext context)
        {
            _context = context;
        }

        // GET: VisaChechers
        public async Task<IActionResult> Index()
        {
              return _context.VisaChechers != null ? 
                          View(await _context.VisaChechers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.VisaChechers'  is null.");
        }

        // GET: VisaChechers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.VisaChechers == null)
            {
                return NotFound();
            }

            var visaChecher = await _context.VisaChechers
                .FirstOrDefaultAsync(m => m.VisaChecherId == id);
            if (visaChecher == null)
            {
                return NotFound();
            }

            return View(visaChecher);
        }

        // GET: VisaChechers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VisaChechers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisaChecherId,Cvc,CardHolderName,CardNumber,Balance")] VisaChecher visaChecher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visaChecher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(visaChecher);
        }

        // GET: VisaChechers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.VisaChechers == null)
            {
                return NotFound();
            }

            var visaChecher = await _context.VisaChechers.FindAsync(id);
            if (visaChecher == null)
            {
                return NotFound();
            }
            return View(visaChecher);
        }

        // POST: VisaChechers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("VisaChecherId,Cvc,CardHolderName,CardNumber,Balance")] VisaChecher visaChecher)
        {
            if (id != visaChecher.VisaChecherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visaChecher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaChecherExists(visaChecher.VisaChecherId))
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
            return View(visaChecher);
        }

        // GET: VisaChechers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.VisaChechers == null)
            {
                return NotFound();
            }

            var visaChecher = await _context.VisaChechers
                .FirstOrDefaultAsync(m => m.VisaChecherId == id);
            if (visaChecher == null)
            {
                return NotFound();
            }

            return View(visaChecher);
        }

        // POST: VisaChechers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.VisaChechers == null)
            {
                return Problem("Entity set 'ModelContext.VisaChechers'  is null.");
            }
            var visaChecher = await _context.VisaChechers.FindAsync(id);
            if (visaChecher != null)
            {
                _context.VisaChechers.Remove(visaChecher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisaChecherExists(decimal id)
        {
          return (_context.VisaChechers?.Any(e => e.VisaChecherId == id)).GetValueOrDefault();
        }
    }
}
