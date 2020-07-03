using System;
using System.ComponentModel.DataAnnotations;

namespace CheerMeApp.Contracts.V1.Requests
{
    public class CreateReplyRequest
    {
        [Required] public string CommentText { get; set; }
        [Required] public Guid ParentId { get; set; }
    }
}