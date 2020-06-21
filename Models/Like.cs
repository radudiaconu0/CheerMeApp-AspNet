using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheerMeApp.Models
{
    public class Like
    {
        [Key] public Guid Id { get; set; }
        public string LikerId { get; set; }
        public string LikableType { get; set; }
        public string LikableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(LikerId))] 
        public User Liker { get; set; }
    }
}