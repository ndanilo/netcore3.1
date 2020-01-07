using System;
using System.Collections.Generic;
using System.Text;
using Infra.Data.Context;
using Infra.Data.Impl.Core;
using Infra.Data.Interfaces.Repositories;
using Models.Entities;

namespace Infra.Data.Impl.Repositories
{
    public class ApplicationCredentialRepository : Repository<ApplicationCredential>, IApplicationCredentialRepository
    {
        public ApplicationCredentialRepository(ApplicationDbContext ctx) : base(ctx)
        {
        }
    }
}
