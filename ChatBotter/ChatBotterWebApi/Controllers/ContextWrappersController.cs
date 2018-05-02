using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using CBLib.Entities;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/ContextWrappers")]
    public class ContextWrappersController : Controller, IAccessVerifier
    {

        private ChatBotContext _dbContext;
        private readonly ILogger _logger;
        private IPatternsRepository _patternRepo;

        public ContextWrappersController(ChatBotContext ctx, ILogger logger, IPatternsRepository patternRepo)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
        }


        [HttpGet]
        [Route("GetContextWrapper/{contextId}")]
        public async Task<IActionResult> GetContextWrapper(int contextId)
        {
            var res = await _patternRepo.GetContextAsync(contextId);
            return Ok();
        }

        [HttpGet]
        [Route("GetAllProjectContextWrappers")]
        public IActionResult GetAllProjectContextWrappers(int projectId)
        {
            return Ok(_patternRepo.GetAllProjectContextsAsync(projectId);
        }

        [HttpPost]
        [Route("AddContextWrapper")]
        public async IActionResult AddContextWrapper()
        {

        }

        [HttpPost]
        [Route("UpdateContextWrapepr")]
        public async IActionResult UpdateContextWrapepr()
        {

        }

        [HttpGet]
        [Route("RemoveContextWrapepr")]
        public async IActionResult RemoveContextWrapper()
        {

        }

        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}
