using Models.Interfaces.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Impl.Core
{
    public class Entity : IEntity
    {
        [Column(TypeName = "bigint")]
        public long Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        [NotMapped]
        public virtual bool IsVirtualDeleted { get; set; } = true;
    }
}
