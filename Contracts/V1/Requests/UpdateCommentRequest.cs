using System.ComponentModel.DataAnnotations;

namespace CheerMeApp.Contracts.V1.Requests
{
    public class UpdateCommentRequest
    {
        [Required] public string CommentText { get; set; }
    }
}