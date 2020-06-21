using System;
using System.ComponentModel.DataAnnotations;

namespace CheerMeApp.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string PostText { get; set; }
    }
}