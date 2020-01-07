using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Data.Interfaces.Core
{
    public interface ITransaction : IDisposable
    {
        Task CommitAsync(CancellationToken ct);
    }
}
