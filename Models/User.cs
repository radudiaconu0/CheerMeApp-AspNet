using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Follower> Followers { get; set; }
        public List<Follower> Followings { get; set; }
    }
}