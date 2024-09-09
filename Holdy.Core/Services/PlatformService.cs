using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.Domain.UnitOfWorkContracts;
using Holdy.Holdy.Core.DTO;
using Holdy.Holdy.Core.ServiceContracts;

namespace Holdy.Holdy.Core.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlatformService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePlatformAsync(Platform platform, string userEmail)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(userEmail);
            if (user != null)
            {
                platform.UserId = user.Id;
                await _unitOfWork.Platforms.InsertAsync(platform);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Platform?>?> GetAllPlatforms(string userId)
        {
            IEnumerable<Platform?>? platforms = await _unitOfWork.Platforms.SelectListByMatchAsync(p => p.UserId.ToString() == userId, "Accounts");
            if (platforms == null)
            {
                return null;
            }
            return platforms.ToList();
        }
        
        public async Task<Platform?> GetPlatformByIdAsync(int platformId, string userId)
        {
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(platformId, "Accounts");
            if (platform == null || platform.UserId.ToString() != userId)
            {
                return null;
            }
            return platform;
        }

        public async Task<bool> DeletePlatformAsync(int id, string userId)
        {
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(id);
            if (platform != null && platform.UserId.ToString() == userId)
            {
                await _unitOfWork.Platforms.DeleteAsync(platform);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePlatformAsync(Platform newEntity, string userEmail)
        {
            User user = (await _unitOfWork.Users.FindByEmailAsync(userEmail))!;
            Platform? oldEntity = await _unitOfWork.Platforms.GetByIdAsync(newEntity.Id);
            if (oldEntity != null && oldEntity.UserId == user.Id)
            {
                newEntity.UserId = user.Id;
                await _unitOfWork.Platforms.UpdateAsync(oldEntity, newEntity);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }
    }
}
