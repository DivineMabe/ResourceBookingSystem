using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.Include(b => b.Resource).ToListAsync();
            return View(bookings);
        }

        // GET: Create Booking
        public IActionResult Create()
        {
            // Only available resources (IsAvailable = true)
            var resources = _context.Resources
                .Where(r => r.IsAvailable)
                .ToList();

            ViewBag.Resources = resources;

            return View();
        }


        // POST: Create Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            ViewBag.Resources = _context.Resources.Where(r => r.IsAvailable).ToList();

            if (!ModelState.IsValid)
                return View(booking);

            // 🧠 CONFLICT CHECK
            bool conflictExists = _context.Bookings.Any(b =>
                b.ResourceId == booking.ResourceId &&
                ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                 (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                 (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime))
            );

            if (conflictExists)
            {
                ModelState.AddModelError("", "❌ This resource is already booked during the selected time.");
                return View(booking);
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resources == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        // POST: Resources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            if (id != resource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceExists(resource.Id))
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
            return View(resource);
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resources == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
