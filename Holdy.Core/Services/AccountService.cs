using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.Domain.UnitOfWorkContracts;
using Holdy.Holdy.Core.DTO;
using Holdy.Holdy.Core.ServiceContracts;
using Holdy.Holdy.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Holdy.Holdy.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAccountAsync(DtoCreateAccountRequest request, string userId)
        {
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync((int)request.PlatformId!);
            if (platform != null && platform.UserId.ToString() == userId)
            {
                Account account = new Account
                {
                    AccountName = request.AccountName,
                    PlatformId = (int)request.PlatformId!
                };
                await _unitOfWork.Accounts.InsertAsync(account);
                if (request.AccountAttributes != null)
                {
                    account.AccountAttributes = request.AccountAttributes.Select(field
                        => new AccountAttribute
                        {
                            AccountId = account.Id,
                            Key = EncryptionHelper.Encrypt(field.Key!),
                            Value = EncryptionHelper.Encrypt(field.Value!),
                            IsSensitive = field.IsSensitive
                        }).ToList();
                }

                await _unitOfWork.SaveAsync();
                return account.Id;
            }

            return -1;
        }

        public async Task<ICollection<DtoAccount?>?> GetAccountsByPlatformIdAsync(int? requestPlatformId, string userId)
        {
            Platform? platform =
                await _unitOfWork.Platforms.GetByIdAsync((int)requestPlatformId!, nameof(AppDbContext.Accounts));
            if (platform != null && platform.UserId.ToString() == userId)
            {
                IEnumerable<Account?>? accounts =
                    await _unitOfWork.Accounts.SelectListByMatchAsync(a => a.PlatformId == requestPlatformId,
                        nameof(AppDbContext.AccountAttributes));
                if (accounts != null)
                {
                    return accounts.Select(account => new DtoAccount
                    {
                        AccountId = account!.Id,
                        AccountName = account.AccountName,
                        PlatformId = account.PlatformId,
                        AccountAttributes = account.AccountAttributes!.Select(aa
                            => new DtoAccountAttribute
                            {
                                Key = EncryptionHelper.Decrypt(aa.Key!),
                                Value = EncryptionHelper.Decrypt(aa.Value!),
                                IsSensitive = aa.IsSensitive
                            }).ToList()
                    }).ToList()!;
                }
            }

            return null;
        }

        public async Task<DtoAccount?> GetAccountByIdAsync(int accountId, string userId)
        {
            Account? account =
                await _unitOfWork.Accounts.GetByIdAsync(accountId, nameof(AppDbContext.AccountAttributes));
            if (account == null)
            {
                return null;
            }

            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account!.PlatformId);
            if (platform != null && platform.UserId.ToString() == userId)
            {
                DtoAccount dtoAccount = new DtoAccount
                {
                    AccountId = account.Id,
                    AccountName = account.AccountName,
                    PlatformId = account.PlatformId,
                    AccountAttributes = account.AccountAttributes!.Select(aa
                                           => new DtoAccountAttribute
                                           {
                            Key = EncryptionHelper.Decrypt(aa.Key!),
                            Value = EncryptionHelper.Decrypt(aa.Value!),
                            IsSensitive = aa.IsSensitive
                        }).ToList()
                };
                return dtoAccount;
            }

            return null;
        }

        public async Task<bool> DeleteAccountAsync(int accountId, string userId)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account != null)
            {
                Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account.PlatformId);
                if (platform!.UserId.ToString() == userId)
                {
                    await _unitOfWork.Accounts.DeleteAsync(account);
                    await _unitOfWork.SaveAsync();
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAccountAsync(DtoUpdateAccountRequest request, string userId)
        {
            Account? account =
                await _unitOfWork.Accounts.GetByIdAsync((int)request.AccountId!,
                    nameof(AppDbContext.AccountAttributes));
            if (account != null)
            {
                Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account.PlatformId);
                if (platform!.UserId.ToString() == userId)
                {
                    if (request.AccountName != null)
                    {
                        account.AccountName = request.AccountName;
                    }

                    if (request.AccountAttributes != null)
                    {
                        account.AccountAttributes!.Clear();
                        foreach (var field in request.AccountAttributes)
                        {
                            account.AccountAttributes!.Add(new AccountAttribute
                            {
                                AccountId = account.Id,
                                Key = EncryptionHelper.Encrypt(field.Key!),
                                Value = EncryptionHelper.Encrypt(field.Value!),
                                IsSensitive = field.IsSensitive
                            });
                        }
                    }

                    await _unitOfWork.SaveAsync();
                    return true;
                }
            }

            return false;
        }
    }
}