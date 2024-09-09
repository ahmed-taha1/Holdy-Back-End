using Holdy.Holdy.Core.Domain.Entities;

namespace Holdy.Holdy.Core.ServiceContracts
{
    public interface IJwtService
    {
        public string GenerateToken(User user, DateTime? customExpirationDate);
        public bool ValidateToken(string token);
        public string? GetEmailFromToken(string token);
        public string? GetIdFromToken(string token);
    }
}