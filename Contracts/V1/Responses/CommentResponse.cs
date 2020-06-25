using System;

namespace CheerMeApp.Contracts.V1.Responses
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; }
        public UserResponse User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}