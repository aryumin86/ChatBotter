using System;
using System.Security.Claims;
using CBLib;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Farewells")]
    public class FarewellsController : Controller, IAccessVerifier
    {
        private ChatBotContext _dbContext;

        public FarewellsController(ChatBotContext ctx)
        {
            _dbContext = ctx;
        }

        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId == currentUserId)
                return true;

            if (_dbContext.Users.Find(currentUserId).AppAdmin)
                return true;

            return false;
        }
    }
}
