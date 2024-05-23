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
    public class VisainfoesController : Controller
    {
        private readonly ModelContext _context;

        public VisainfoesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Visainfoes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Visainfos.Include(v => v.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Visainfoes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Visainfos == null)
            {
                return NotFound();
            }

            var visainfo = await _context.Visainfos
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (visainfo == null)
            {
                return NotFound();
            }

            return View(visainfo);
        }

        // GET: Visainfoes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Visainfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,Cvc,CardHolderName,CardNumber,UserId")] Visainfo visainfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visainfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", visainfo.UserId);
            return View(visainfo);
        }

        // GET: Visainfoes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Visainfos == null)
            {
                return NotFound();
            }

            var visainfo = await _context.Visainfos.FindAsync(id);
            if (visainfo == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", visainfo.UserId);
            return View(visainfo);
        }

        // POST: Visainfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("PaymentId,Cvc,CardHolderName,CardNumber,UserId")] Visainfo visainfo)
        {
            if (id != visainfo.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visainfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisainfoExists(visainfo.PaymentId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", visainfo.UserId);
            return View(visainfo);
        }

        // GET: Visainfoes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Visainfos == null)
            {
                return NotFound();
            }

            var visainfo = await _context.Visainfos
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (visainfo == null)
            {
                return NotFound();
            }

            return View(visainfo);
        }

        // POST: Visainfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Visainfos == null)
            {
                return Problem("Entity set 'ModelContext.Visainfos'  is null.");
            }
            var visainfo = await _context.Visainfos.FindAsync(id);
            if (visainfo != null)
            {
                _context.Visainfos.Remove(visainfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisainfoExists(decimal id)
        {
          return (_context.Visainfos?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        }
    }
}
