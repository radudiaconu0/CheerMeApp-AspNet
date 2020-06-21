using System;
using CheerMeApp.Models;

namespace CheerMeApp.Contracts.V1.Responses
{
    public class LikeResponse
    {
        public UserResponse Liker { get; set; }
    }
}