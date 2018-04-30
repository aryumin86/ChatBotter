using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/ContextWrappers")]
    public class ContextWrappersController : Controller, IAccessVerifier
    {

        private ChatBotContext _dbContext;
        private readonly ILogger _logger;

        public ContextWrappersController(ChatBotContext ctx, ILogger logger)
        {
            _dbContext = ctx;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetContextWrapper")]
        public async Task<IActionResult> GetContextWrapper()
        {

        }

        [HttpGet]
        [Route("GetAllProjectContextWrappers")]
        public async Task<IActionResult> GetAllProjectContextWrappers()
        {

        }

        [HttpPost]
        [Route("AddContextWrapper")]
        public async Task<IActionResult> AddContextWrapper()
        {

        }

        [HttpPost]
        [Route("UpdateContextWrapepr")]
        public async Task<IActionResult> UpdateContextWrapepr()
        {

        }

        [HttpGet]
        [Route("RemoveContextWrapepr")]
        public async Task<IActionResult> RemoveContextWrapepr()
        {

        }

        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}
