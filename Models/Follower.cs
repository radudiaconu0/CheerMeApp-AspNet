using System;
using System.ComponentModel.DataAnnotations.Schema;
using CheerMeApp.Contracts.V1.Responses;

namespace CheerMeApp.Models
{
    public class Follower
    {
        public Guid Id { get; set; }
        public string FollowerId { get; set; }
        public string FollowableId { get; set; }
        public bool Accepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User FollowerUser  { get; set; }
        public User FollowingUser { get; set; }
    }
}