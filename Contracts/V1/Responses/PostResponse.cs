using System;
using System.Runtime.InteropServices.ComTypes;
using CheerMeApp.Models;

namespace CheerMeApp.Contracts.V1.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string PostText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public UserResponse User { get; set; }
        public int LikesCount { get; set; }
        public bool Liked { get; set; }
    }
}