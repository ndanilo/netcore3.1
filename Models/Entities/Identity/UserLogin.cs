using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<long>
    {
        //[Key, Column(TypeName = "bigint")]
        //public long Id { get; set; }
    }
}
