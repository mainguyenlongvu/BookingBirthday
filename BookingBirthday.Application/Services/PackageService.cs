using BookingBirthday.Application.IServices;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingBirthday.Application.Services
{
    public class PackageService : IPackageService

    {
        private readonly BookingDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public PackageService(BookingDbContext context) 
        {
            _context=context;
        }

        public List<Package> GetAllPackages()
        {
            //int page = 1;
            //var allPackages = _context.Packages.Include(hh => hh.Promotion).AsQueryable();

            //var result = PaginatedList<Package>.Create(allPackages, page, PAGE_SIZE);
            var result = _context.Packages.Select(hh => new Package
            {
                Id = hh.Id,
                Name = hh.Name,
                Price = hh.Price,
                Venue = hh.Venue,
                Detail = hh.Detail,
               PromotionId = hh.PromotionId,
            });
            return result.ToList();
            
        }


        public Task<int> AddPackageAsync(Package model)
        {
            throw new NotImplementedException();
        }

        public Task DeletePackage(int id)
        {
            throw new NotImplementedException();
        }



        public Task<Package>? GetPackage(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePackage(int id, Package model)
        {
            throw new NotImplementedException();
        }
    }
}
