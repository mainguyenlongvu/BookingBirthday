using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using BookingBirthday.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.IServices
{
    public interface IUserService
    {
        Task<UserModel> LoginUser(string username, string password);
        Task<bool> ChangePassword(User user, string newPassword);
        Task<UserModel> AddUser(User user);
        Task<UserModel> GetUserByUsername(string username);
        Task<List<UserModel>> GetManagerUsers();
        Task<List<UserModel>> GetStudentUsers();
        Task<List<UserModel>> GetAllUsers();
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateRole(User user, Role role);
    }
}
