using BookingBirthday.Application.IServices;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using BookingBirthday.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.Services
{
    public class UserService : IUserService
    {
        
        private readonly BookingDbContext _context;
        public UserService(BookingDbContext context)
        {
            _context = context;
        }

        public Task<UserModel> AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePassword(User user, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserModel>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserModel>> GetManagerUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserModel>> GetStudentUsers()
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> LoginUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRole(User user, Role role)
        {
            throw new NotImplementedException();
        }
    }
}
        


