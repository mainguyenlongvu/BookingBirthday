using BookingBirthday.Application.IServices;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.Services
{
    public class PackageService : IPackageService

    {
        private readonly BookingDbContext _context;

        public PackageService(BookingDbContext context) 
        {
            _context = context;
        }
        

        public async Task<int> AddPackageAsync(Package model)
        {
            var newPackage = model;
            _context.Packages!.Add(newPackage);
            await _context.SaveChangesAsync();

            return newPackage.Id;
        }

        public async Task DeletePackageAsync(int id)
        {
            var deletePackage = _context.Packages!.SingleOrDefault(b => b.Id == id);
            if (deletePackage != null)
            {
                _context.Packages!.Remove(deletePackage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            var packages = await _context.Packages!.ToListAsync();
            return packages;
        }

        public async Task<Package> GetPackageAsync(int id)
        {
            var package = await _context.Packages!.FindAsync(id);
            return package!;
        }

        public async Task UpdatePackageAsync(int id, Package model)
        {
            if (id == model.Id)
            {
                var updateBook = model;
                _context.Packages!.Update(updateBook);
                await _context.SaveChangesAsync();
            }
        }
    }
}
