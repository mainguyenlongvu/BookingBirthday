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
        public Task<List<Package>> GetAllPackagesAsync();
        public Task<Package> GetPackageAsync(int id);
        public Task<int> AddPackageAsync(Package model);
        public Task UpdatePackageAsync(int id, Package model);
        public Task DeletePackageAsync(int id);
    }
}
