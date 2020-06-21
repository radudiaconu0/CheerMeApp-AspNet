using System;
using System.Collections.Generic;
using CheerMeApp.Models;

namespace CheerMeApp.Contracts.V1.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDay { get; set; }
        public string ProfileImage { get; set; }
        public Genders Gender { get; set; }
    }
}