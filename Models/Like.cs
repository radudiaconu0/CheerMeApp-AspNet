using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheerMeApp.Models
{
    public class Like
    {
        [Key] public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(UserId))] public User Liker { get; set; }
        [ForeignKey(nameof(PostId))] public Post Post { get; set; }
        [ForeignKey(nameof(CommentId))] public Comment Comment { get; set; }
    }
}