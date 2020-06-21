using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CheerMeApp.Models
{
    public enum Genders
    {
        Male,
        Female
    }

    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public string ProfileImage { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}