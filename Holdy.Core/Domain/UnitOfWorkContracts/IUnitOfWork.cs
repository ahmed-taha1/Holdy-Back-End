using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Identity;

namespace Holdy.Holdy.Core.Domain.UnitOfWorkContracts
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public IRepository<OTP> OTPs { get; }
        public SignInManager<User> SignInManager { get; }
        Task SaveAsync();
    }
}
