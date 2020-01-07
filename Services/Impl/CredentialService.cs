using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infra.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Impl
{
    public class CredentialService : ICredentialService
    {
        private readonly IApplicationCredentialRepository _applicationCredentialRepository;
        public CredentialService(IApplicationCredentialRepository applicationCredentialRepository)
        {
            _applicationCredentialRepository = applicationCredentialRepository;
        }
        public async Task<bool> IsApplicationAuthorized(string apiKey)
        {
            try
            {
                var guidApiKey = new Guid(apiKey);
                var applicationCredentials = await _applicationCredentialRepository.FirstOrDefaultAsync(a => a.ApiKey == guidApiKey);
                return applicationCredentials != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
