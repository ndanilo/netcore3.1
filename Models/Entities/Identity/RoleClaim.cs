using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
    }
}
