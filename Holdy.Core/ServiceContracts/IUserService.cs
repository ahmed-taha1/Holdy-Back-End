using Holdy.Holdy.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Holdy.Holdy.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<bool> LoginAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(string oldPassword, string newPassword, string email);
        Task<bool> UpdatePasswordAsync(string newPassword, string email);
        Task<bool> SetPinAsync(string pin, string userEmail);
        Task<bool> CheckPinAsync(string pin, string userEmail);
        Task<bool> RemoveAccountAsync(string password, string email);
    }
}