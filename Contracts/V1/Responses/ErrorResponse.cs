using System.Collections.Generic;
using CheerMeApp.Models;

namespace CheerMeApp.Contracts.V1.Responses
{
    public class ErrorResponse
    {
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}