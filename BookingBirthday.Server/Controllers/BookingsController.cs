using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Controllers
{
    public class BookingsController : Controller
    {
        private readonly BookingDbContext _context;

        public BookingsController(BookingDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookingDbContext = _context.Bookings.Include(b => b.Bill).Include(b => b.Guest).Include(b => b.Host).Include(b => b.Payment);
            return View(await bookingDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Bill)
                .Include(b => b.Guest)
                .Include(b => b.Host)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["BillId"] = new SelectList(_context.Bills, "Id", "Id");
            ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "Address");
            ViewData["HostId"] = new SelectList(_context.Hosts, "Id", "Address");
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BookingStatus,Total,GuestId,HostId,PaymentId,BillId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillId"] = new SelectList(_context.Bills, "Id", "Id", booking.BillId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "Address", booking.GuestId);
            ViewData["HostId"] = new SelectList(_context.Hosts, "Id", "Address", booking.HostId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", booking.PaymentId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["BillId"] = new SelectList(_context.Bills, "Id", "Id", booking.BillId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "Address", booking.GuestId);
            ViewData["HostId"] = new SelectList(_context.Hosts, "Id", "Address", booking.HostId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", booking.PaymentId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,BookingStatus,Total,GuestId,HostId,PaymentId,BillId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["BillId"] = new SelectList(_context.Bills, "Id", "Id", booking.BillId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "Address", booking.GuestId);
            ViewData["HostId"] = new SelectList(_context.Hosts, "Id", "Address", booking.HostId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", booking.PaymentId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Bill)
                .Include(b => b.Guest)
                .Include(b => b.Host)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
