using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Infra.Data.Auth;
using Infra.Data.Interfaces.Core;
using Infra.Data.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DTO.Development;
using Models.DTO.Development.User;
using Models.DTO.Exceptions;
using Models.DTO.Generics;
using Models.Entities;
using Models.Entities.Identity;
using NetCore31Api.Middlewares;

namespace NetCore31Api.Controllers
{
    /// <summary>
    /// Development class
    /// </summary>
    [Area("api")]
    [ApiExplorerSettings(GroupName = "api")]
    [Route("[area]/development")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class DevelopmentController : ControllerBase
    {
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unit;
        private readonly IApplicationCredentialRepository _applicationCredentialRepository;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tokenConfigurations"></param>
        /// <param name="signingConfigurations"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="unit"></param>
        /// <param name="applicationCredentialRepository"></param>
        public DevelopmentController(TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations, UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unit, IApplicationCredentialRepository applicationCredentialRepository)
        {
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
            _userManager = userManager;
            _roleManager = roleManager;
            _unit = unit;
            _applicationCredentialRepository = applicationCredentialRepository;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateToken(User user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(Guid.NewGuid().ToString(), "Identity"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                }
            );

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));

            var creationDate = DateTime.UtcNow;
            var expirationDate = DateTime.UtcNow + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = creationDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        /// <summary>
        /// first test method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var response = new Sample
            {
                Message = "first-method works!"
            };
            return Ok(response);
        }

        /// <summary>
        /// post file test method
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] SampleFile file, CancellationToken ct)
        {
            var x = file;
            return Ok("file upload works!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-token")]
        public async Task<IActionResult> GeneratesToken([FromBody] Login model, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new NotFoundException("Email and/or password supplied is incorrect");

            var authenticated = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!authenticated)
                throw new BadRequestException("Invalid credentials");

            var token = GenerateToken(user);
            return Ok(token);
        }

        /// <summary>
        /// test authentication
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("test-auth")]
        [Authorize(Policy = "Bearer")]
        public IActionResult TestAuth()
        {
            return Ok("authentication works!");
        }

        /// <summary>
        /// test role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("test-role")]
        [Authorize(Policy = "Bearer", Roles = "Administrator")]
        public IActionResult TestRole()
        {
            return Ok("user is authorized");
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-user")]
        [ApplicationAuthorizationFilter]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser model, CancellationToken ct)
        {
            using (var transaction = await _unit.BeginTransactionAsync(ct))
            {
                var user = new User
                {
                    Active = true,
                    UserName = model.Email,
                    Email = model.Email,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    throw new ConflictException("dados de usuário já existentes na aplicação");

                var basicProfile = "Basic";
                var adminProfile = "Administrator";

                if (!await _roleManager.RoleExistsAsync(basicProfile))
                {
                    var role = new Role();
                    role.Name = basicProfile;
                    role.Active = true;
                    role.LastUpdatedAt = DateTime.UtcNow;
                    role.CreatedAt = DateTime.UtcNow;

                    IdentityResult roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                        throw new InternalServerError("Erro ao criar perfil básico");
                }

                if (!await _roleManager.RoleExistsAsync(adminProfile))
                {
                    var role = new Role();
                    role.Name = adminProfile;
                    role.Active = true;
                    role.LastUpdatedAt = DateTime.UtcNow;
                    role.CreatedAt = DateTime.UtcNow;

                    IdentityResult roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                        throw new InternalServerError("Erro ao criar perfil administrator");
                }

                await _userManager.AddToRoleAsync(user, basicProfile);

                await transaction.CommitAsync(ct);
            }
            
            var response = new Sample
            {
                Message = "user created"
            };

            return Ok(response);
        }

        /// <summary>
        /// returns a list of authorized customers with application credential access for application
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-app-credentials-list")]
        public async Task<IActionResult> GetApplicationCredentialsList(CancellationToken ct)
        {
            var response = await _applicationCredentialRepository.Select(c => c.ApplicationName).ToListAsync(ct);
            return Ok(response);
        }
    }
}