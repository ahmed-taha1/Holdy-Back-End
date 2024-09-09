﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holdy.Holdy.Core.Domain.Entities
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? PlatformName { get; set; }
        [Required]
        [StringLength(100)]
        public string? IconColor { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Account>? Accounts { get; set; } = new List<Account>();
    }
}