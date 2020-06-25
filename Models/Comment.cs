using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace CheerMeApp.Models
{
    public class Comment
    {
        [Key] public Guid Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        [ForeignKey(nameof(Id))] public Guid? ParentId { get; set; }
        [ForeignKey(nameof(UserId))] public User User { get; set; }
        [ForeignKey(nameof(PostId))] public Post Post { get; set; }
        [ForeignKey(nameof(ParentId))] public Comment Parent { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}