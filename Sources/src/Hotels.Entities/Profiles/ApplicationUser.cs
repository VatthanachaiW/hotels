using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Hotels.Entities.Profiles
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [NotMapped] public string Fullname => $"{Firstname} {Lastname}";
    }
}