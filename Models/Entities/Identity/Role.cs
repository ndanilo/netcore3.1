using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Models.Interfaces.Core;

namespace Models.Entities.Identity
{
    public class Role : IdentityRole<long>, IEntity
    {
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        [NotMapped]
        public virtual bool IsVirtualDeleted { get; set; } = true;
    }
}
