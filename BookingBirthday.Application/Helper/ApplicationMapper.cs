using AutoMapper;
using BookingBirthday.Data.Entities;
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
            CreateMap<Package, Package>().ReverseMap();
            CreateMap<User, User>().ReverseMap();
        }
    }
}
