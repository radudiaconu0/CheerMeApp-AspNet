using System;
using System.Net.Http;
using System.Threading.Tasks;
using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;
using Microsoft.AspNetCore.Http;

namespace CheerMeApp.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string firstname, string lastname, DateTime birthDay, string gender, string username);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}