using BookingBirthday.Application.IServices;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Models;
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

        public List<PackageModel> GetAllPackages()
        {
            //int page = 1;
            //var allPackages = _context.Packages.Include(hh => hh.Promotion).AsQueryable();

            //var result = PaginatedList<Package>.Create(allPackages, page, PAGE_SIZE);
            var result = _context.Packages.Select(hh => new PackageModel
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


        public Task<int> AddPackageAsync(PackageModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeletePackage(int id)
        {
            throw new NotImplementedException();
        }



        public Task<PackageModel>? GetPackage(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePackage(int id, PackageModel model)
        {
            throw new NotImplementedException();
        }
    }
}
