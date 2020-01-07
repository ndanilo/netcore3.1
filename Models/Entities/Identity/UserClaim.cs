using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<long>
    {
    }
}
