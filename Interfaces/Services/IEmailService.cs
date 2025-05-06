using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Interfaces.Services
{
    public interface IEmailService
    {
        Task UpdateUserEmailAsync(int userId, string email);
    }
}
