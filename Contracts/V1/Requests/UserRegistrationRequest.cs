using System;
using System.ComponentModel.DataAnnotations;
using CheerMeApp.Models;

namespace CheerMeApp.Contracts.V1.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}