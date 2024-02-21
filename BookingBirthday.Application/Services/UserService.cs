using BookingBirthday.Application.IServices;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
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

        public Task<User> AddUser(User user)
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

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetManagerUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetStudentUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> LoginUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRole(User user, Role role)
        {
            throw new NotImplementedException();
        }
    }
}
        


