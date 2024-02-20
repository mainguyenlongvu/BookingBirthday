using AutoMapper;
using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Package, PackageModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
