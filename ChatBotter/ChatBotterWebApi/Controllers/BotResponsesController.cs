using System;
using System.Security.Claims;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotterWebApi.Controllers
{
    public class BotResponsesController : Controller, IAccessVerifier
    {
        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}
