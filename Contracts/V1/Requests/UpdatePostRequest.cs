using System.ComponentModel.DataAnnotations;

namespace CheerMeApp.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        public string PostText { get; set; }
    }
}