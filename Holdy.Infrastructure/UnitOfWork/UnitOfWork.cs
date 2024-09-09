using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.Domain.RepositoryContracts;
using Holdy.Holdy.Core.Domain.UnitOfWorkContracts;
using Holdy.Holdy.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Holdy.Holdy.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext.AppDbContext _dp;
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; private set; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public IRepository<OTP> OTPs { get; }
        public SignInManager<User> SignInManager { get; }
        public UnitOfWork(AppDbContext.AppDbContext dp, UserManager<User> users, SignInManager<User> signInManager)
        {
            _dp = dp;
            Accounts = new Repository<Account>(_dp);
            Users = users;
            SignInManager = signInManager;
            AccountAttributes = new Repository<AccountAttribute>(_dp);
            Platforms = new Repository<Platform>(_dp);
            OTPs = new Repository<OTP>(_dp);
        }
        public async Task SaveAsync()
        {
            await _dp.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dp.Dispose();
        }
    }
}
