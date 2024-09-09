﻿using Holdy.Holdy.Core.DTO;

namespace Holdy.Holdy.Core.ServiceContracts
{
    public interface IAccountService
    {
        Task<int> CreateAccountAsync(DtoCreateAccountRequest request, string userId);
        Task<ICollection<DtoAccount?>?> GetAccountsByPlatformIdAsync(int? requestPlatformId, string userId);
        Task<DtoAccount?> GetAccountByIdAsync(int accountId, string userId);
        Task<bool> DeleteAccountAsync(int accountId, string userId);
        Task<bool> UpdateAccountAsync(DtoUpdateAccountRequest request, string userId);
    }
}