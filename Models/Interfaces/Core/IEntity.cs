using System;

namespace Models.Interfaces.Core
{
    public interface IEntity
    {
        long Id { get; set; }
        bool Active { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime LastUpdatedAt { get; set; }
        bool IsVirtualDeleted { get; set; }
    }
}
