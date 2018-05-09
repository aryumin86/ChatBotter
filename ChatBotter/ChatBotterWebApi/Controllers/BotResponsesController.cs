using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CBLib;
using CBLib.Entities;
using ChatBotterWebApi.Data;
using ChatBotterWebApi.DTO;
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
        private readonly IMapper _mapper;
        private readonly IValidationHelperRepository _validationRepo;

        public BotResponsesController(ChatBotContext ctx, ILogger<BotResponsesController> logger, 
            IPatternsRepository patternRepo, IMapper mapper, IValidationHelperRepository validationRepo)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
            _mapper = mapper;
            _validationRepo = validationRepo;
        }

        [HttpGet]
        [Route("GetBotResponse")]
        public async Task<IActionResult> GetBotResponse(int respId)
        {
            var resp = await _patternRepo.GetBotResponseAsync(respId);

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
            var res = await _patternRepo.GetAllProjectBotResponsesAsync(prjId);

            if (res == null)
                return NotFound();

            if(!HasAccess(_dbContext.TheProjects.First(p => p.Id == prjId).OwnerId))
                return StatusCode(403);

            return Ok(res);
        }

        [HttpPost]
        [Route("AddBotResponse")]
        public async Task<IActionResult> AddBotResponse([FromBody] BotResponseDto resp)
        {
            if (resp == null)
                return BadRequest();

            if (!HasAccess(_validationRepo.GetContextOwnerId(resp.PatternId)))
                return StatusCode(403);

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(resp, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(resp, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BotResponse respObj = _mapper.Map<BotResponse>(resp);
            if (await _patternRepo.AddBotResponseToPatternAsync(respObj))
                return Ok();
            else
                return BadRequest(); ;
        }

        [HttpPost]
        [Route("UpdateBotResponse")]
        public async Task<IActionResult> UpdateBotResponse([FromBody] BotResponseDto resp)
        {
            if (resp == null)
                return BadRequest();

            if (!HasAccess(_validationRepo.GetContextOwnerId(resp.PatternId)))
                return StatusCode(403);

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(resp, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(resp, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respFromDb = await _patternRepo.GetBotResponseAsync(resp.Id);

            if (respFromDb == null)
                return NotFound("Response wasn't found in database");

            BotResponse respObj = _mapper.Map<BotResponse>(resp);
            if (await _patternRepo.UpdateBotResponseToPatternAsync(respObj))
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("RemoveBotResponse/{respId}")]
        public async Task<IActionResult> RemoveBotResponse(int respId)
        {
            if (!HasAccess(_validationRepo.GetResponseOwnerId(respId)))
                return StatusCode(403);

            var resp = await _dbContext.BotResponses.FirstOrDefaultAsync(r => r.Id == respId);            

            if (resp == null)
                return NotFound("Response wasn't found in database");

            if(await _patternRepo.DeleteBotResponseToPatternAsync(resp))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("GetResponseToUserMessage")]
        public IActionResult GetResponseToUserMessage([FromBody]UserMessage message)
        {
            var resp = _patternRepo.GetReponseToUserMessageAsync(message);

            if (resp == null)
            {
                var defaultResponses = _dbContext.DefaultBotResponses.Where(r => r.TheProjectId == message.ProjectId).ToArray();
                if (defaultResponses.Length == 0)
                    return null;
                Random random = new Random();
                return Ok(defaultResponses[random.Next(defaultResponses.Length - 1)]);
            }

            return Ok(resp.ResponseText);
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
