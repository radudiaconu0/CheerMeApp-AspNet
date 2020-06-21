using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheerMeApp.Models
{
    public class Post
    {
        [Key] public Guid Id { get; set; }
        public string PostText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User User { get; set; }
    }
}