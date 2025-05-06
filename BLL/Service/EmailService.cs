using Interfaces.Services;
using BLL.Repositories;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailRepository _emailRepository;

        public EmailService(string connectionString)
        {
            _emailRepository = new EmailRepository(connectionString);
        }

        public async Task UpdateUserEmailAsync(int userId, string email)
        {
            // Валидация email
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                throw new ArgumentException("Некорректный адрес электронной почты.");
            }

            // Обновление email через репозиторий
            await _emailRepository.UpdateUserEmailAsync(userId, email);
        }

        // Простая валидация email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
