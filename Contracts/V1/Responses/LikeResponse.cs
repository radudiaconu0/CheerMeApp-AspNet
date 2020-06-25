using System;
namespace CheerMeApp.Contracts.V1.Responses
{
    public class LikeResponse
    {
        public DateTime CreatedAt { get; set; }
        public UserResponse Liker { get; set; }
    }
}