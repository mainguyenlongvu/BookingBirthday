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
    public class HostsController : Controller
    {
        private readonly BookingDbContext _context;

        public HostsController(BookingDbContext context)
        {
            _context = context;
        }

        // GET: Hosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hosts.ToListAsync());
        }

        // GET: Hosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _context.Hosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (host == null)
            {
                return NotFound();
            }

            return View(host);
        }

        // GET: Hosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,DateOfBirth,Email,Address,Phone")] Data.Entities.Host host)
        {
            if (ModelState.IsValid)
            {
                _context.Add(host);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(host);
        }

        // GET: Hosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _context.Hosts.FindAsync(id);
            if (host == null)
            {
                return NotFound();
            }
            return View(host);
        }

        // POST: Hosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,DateOfBirth,Email,Address,Phone")] Data.Entities.Host host)
        {
            if (id != host.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(host);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HostExists(host.Id))
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
            return View(host);
        }

        // GET: Hosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _context.Hosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (host == null)
            {
                return NotFound();
            }

            return View(host);
        }

        // POST: Hosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var host = await _context.Hosts.FindAsync(id);
            if (host != null)
            {
                _context.Hosts.Remove(host);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HostExists(int id)
        {
            return _context.Hosts.Any(e => e.Id == id);
        }
    }
}
