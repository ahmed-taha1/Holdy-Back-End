using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace Holdy.Holdy.Core.DTO
{
    public class DtoAccount
    {
        [Required(ErrorMessage = "Account name is required")]
        public String? AccountName { get; set; }
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
        public int? AccountId { get; set; }
        public List<DtoAccountAttribute>? AccountAttributes { get; set; }
    }

    public class DtoAccountAttribute{
        public String? Key { get; set; }
        public String? Value { get; set; }
        public bool IsSensitive { get; set; } = false;
    }

    public class DtoUpdateAccountRequest
    {
        [Required(ErrorMessage = "Account id is required")]
        public int? AccountId { get; set; }
        public String? AccountName { get; set; }
        public List<DtoAccountAttribute>? AccountAttributes { get; set; }
    }

    public class DtoCreateAccountRequest
    {
        [Required(ErrorMessage = "Account name is required")]
        public String? AccountName { get; set; }
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
        public List<DtoAccountAttribute>? AccountAttributes { get; set; }
    }

    public class DtoAccountId
    {
        public int? AccountId { get; set; }
    }

    public class DtoGetAccountsByPlatformIdRequest
    {
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
    }

    public class DtoGetAccountsByPlatformIdResponse
    {
        public ICollection<DtoAccount?>? Accounts { get; set; }
    }

}
