using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;

namespace BookingBirthday.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public PackagesController(BookingDbContext context)
        {
            _context = context;
        }

        // GET: api/Packages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> GetPackages()
        {
            //return await _context.Packages.ToListAsync();
            try
            {
                if (_context.Packages == null)
                {
                    return new NotFoundResult();
                }

                var packages = await _context.Packages.ToListAsync();

                if (packages == null)
                {
                    return NotFound("No package found!");
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // GET: api/Packages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);

            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        // PUT: api/Packages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackage(int id, Package package)
        {
            if (id != package.Id)
            {
                return BadRequest();
            }

            _context.Entry(package).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Packages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Package>> PostPackage(Package package)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackage", new { id = package.Id }, package);
        }

        // DELETE: api/Packages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PackageExists(int id)
        {
            return _context.Packages.Any(e => e.Id == id);
        }
    }
}
