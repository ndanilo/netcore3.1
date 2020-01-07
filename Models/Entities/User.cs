using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Models.Interfaces.Core;

namespace Models.Entities
{
    public class User : IdentityUser<long>, IEntity
    {
        [Key, Column(TypeName = "bigint")]
        public override long Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        [NotMapped]
        public virtual bool IsVirtualDeleted { get; set; } = true;
    }
}
