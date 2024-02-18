using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.IServices
{
    public interface IPackageService
    {
        public List<PackageModel> GetAllPackages();
        public Task<PackageModel>? GetPackage(int id);
        public Task<int> AddPackageAsync(PackageModel model);
        public Task UpdatePackage(int id, PackageModel model);
        public Task DeletePackage(int id);
    }
}
