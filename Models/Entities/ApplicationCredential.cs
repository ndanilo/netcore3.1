using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Models.Impl.Core;

namespace Models.Entities
{
    public class ApplicationCredential : Entity
    {
        [Column(TypeName = "varchar(255)"), Required]
        public string ApplicationName { get; set; }

        [Column(TypeName = "uniqueidentifier"), Required]
        public Guid ApiKey { get; set; }
    }
}
