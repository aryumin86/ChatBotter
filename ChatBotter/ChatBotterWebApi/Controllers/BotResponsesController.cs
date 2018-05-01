using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/BotResponses")]
    public class BotResponsesController : Controller, IAccessVerifier
    {
        private ChatBotContext _dbContext;
        private readonly ILogger _logger;
        private IPatternsRepository _patternRepo;

        public BotResponsesController(ChatBotContext ctx, ILogger logger, IPatternsRepository patternRepo)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
        }

        [HttpGet]
        [Route("GetBotResponse")]
        public async Task<IActionResult> GetBotResponse(int respId)
        {
            var resp = await _dbContext.BotResponses.FirstOrDefaultAsync(r => r.Id == respId);

            if (resp == null)
                return NotFound("no such response in database");

            var prj = await _dbContext.TheProjects.FirstOrDefaultAsync(p => p.Id == resp.TheProjectId);

            if (prj == null)
                return NotFound("no such project in database");

            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            return Ok(resp);
        }

        [HttpGet]
        [Route("GetAllProjectBotResponses/{prjId}")]
        public async Task<IActionResult> GetAllProjectBotResponses(int prjId)
        {
            var res = _dbContext.BotResponses.Where(r => r.TheProjectId == prjId);

            if (res == null)
                return NotFound();

            if(!HasAccess(_dbContext.TheProjects.First(p => p.Id == prjId).OwnerId))
                return StatusCode(403);

            return Ok(res);
        }

        [HttpPost]
        [Route("AddBotResponse")]
        public async Task<IActionResult> AddBotResponse([FromBody] BotResponse resp)
        {
            try
            {
                _dbContext.BotResponses.Add(resp);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant't add bot response to project");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("UpdateBotResponse")]
        public async Task<IActionResult> UpdateBotResponse([FromBody] BotResponse resp)
        {
            var respFromDb = await _dbContext.BotResponses.FirstOrDefaultAsync(r => r.Id == resp.Id);

            if (respFromDb == null)
                return NotFound("Response wasn't found in database");

            if(!HasAccess(_dbContext.Users.Where(u => u.Id == respFromDb.TheProject.Id).First().Id))
                return StatusCode(403);

            try
            {
                _dbContext.Entry(respFromDb).CurrentValues.SetValues(resp);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant't update bot response ({prj.Id})", resp.Id);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("RemoveBotResponse/{respId}")]
        public async Task<IActionResult> RemoveBotResponse(int respId)
        {
            var resp = await _dbContext.BotResponses.FirstOrDefaultAsync(r => r.Id == respId);

            if (resp == null)
                return NotFound("Response wasn't found in database");

            if (!HasAccess(_dbContext.Users.Where(u => u.Id == resp.TheProject.Id).First().Id))
                return StatusCode(403);

            try
            {
                _dbContext.BotResponses.Remove(resp);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant't remove response with ID ({prjId})", respId);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("GetResponseToUserMessage")]
        public async Task<string> GetResponseToUserMessage([FromBody]UserMessage message)
        {
            return await _patternRepo.GetReponseToUserMessage(message);
        }


        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}
