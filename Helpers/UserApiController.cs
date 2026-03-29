using AskAgainApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AskAgainApi.Helpers
{
    public class UserApiController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.Thumbprint);

            return userId == null ? throw new HttpException("Authorize is incorrect.", 401) : Guid.Parse(userId);
        }
        protected string GetUserName()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            return userName ?? throw new HttpException("Authorize is incorrect.", 401);
        }

        protected string GetUserEmail()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            return userEmail ?? throw new HttpException("Authorize is incorrect.", 401);
        }
    }
}
