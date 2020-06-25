using System;
using System.Linq;
using System.Security.Claims;
using CheerMeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;

namespace CheerMeApp.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User == null
                ? string.Empty
                : httpContext.User.Claims.Single(claim => claim.Type == "id").Value;
        }
    }
}