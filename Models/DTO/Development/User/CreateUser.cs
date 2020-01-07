using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DTO.Development.User
{
    public class CreateUser
    {
        public string Email { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords doesn't match'")]
        public string PasswordConfirmation { get; set; }
    }
}
