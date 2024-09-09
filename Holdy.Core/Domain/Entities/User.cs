using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Holdy.Holdy.Core.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(500)]
        public string? PinHash { get; set; }
        public virtual List<Platform>? Platforms { get; set; }
    }
}