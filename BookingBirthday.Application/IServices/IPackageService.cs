using BookingBirthday.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.IServices
{
    public interface IPackageService
    {
        public List<Package> GetAllPackages();
        public Task<Package>? GetPackage(int id);
        public Task<int> AddPackageAsync(Package model);
        public Task UpdatePackage(int id, Package model);
        public Task DeletePackage(int id);
    }
}
