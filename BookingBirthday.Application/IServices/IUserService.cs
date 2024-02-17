using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
        Task<bool> ChangePassword(User user, string newPassword);
        Task<User> AddUser(User user);
        Task<User> GetUserByUsername(string username);
        Task<List<User>> GetManagerUsers();
        Task<List<User>> GetStudentUsers();
        Task<List<User>> GetAllUsers();
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateRole(User user, Role role);
    }
}
