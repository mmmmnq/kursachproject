
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Interfaces.Services
{
    public interface IUserService
    {
        bool AuthenticateUser(string username, string password, out string role);
        int GetUserId(string username);
        Task<List<UserDTO>> GetUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> CreateUserAsync(UserDTO user);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> AddUserAsync(UserDTO user);

    }
}
