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
using ChatBotterWebApi.DTO;
using AutoMapper;
using ChatBotterWebApi.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        private readonly IMapper _mapper;
        private readonly IValidationHelperRepository _validationRepo;

        public ContextWrappersController(ChatBotContext ctx, ILogger<ContextWrappersController> logger, 
            IPatternsRepository patternRepo, IMapper mapper, IValidationHelperRepository validationRepo)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
            _mapper = mapper;
            _validationRepo = validationRepo;
        }


        [HttpGet]
        [Route("GetContextWrapper/{contextId}")]
        public async Task<IActionResult> GetContextWrapper(int contextId)
        {
            var res = await _patternRepo.GetContextAsync(contextId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllProjectContextWrappers")]
        public async Task<IActionResult> GetAllProjectContextWrappers(int projectId)
        {
            var res = await _patternRepo.GetAllProjectContextsAsync(projectId);
            return Ok(res);
        }

        [HttpPost]
        [Route("AddContextWrapper")]
        public async Task<IActionResult> AddContextWrapper([FromBody]ContextDto context)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(context, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(context, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasAccess(_validationRepo.GetProjectOwnerId(context.ProjectId)))
                return StatusCode(403);

            var contextObj = _mapper.Map<ContextWrapper>(context);
            if(await _patternRepo.AddContextAsync(contextObj))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("UpdateContextWrapepr")]
        public async Task<IActionResult> UpdateContextWrapepr([FromBody]ContextDto context)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(context, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(context, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasAccess(_validationRepo.GetContextOwnerId(context.Id)))
                return StatusCode(403);

            var contextObj = _mapper.Map<ContextWrapper>(context);
            if(await _patternRepo.UpdateContextAsync(contextObj))
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("RemoveContextWrapepr")]
        public async Task<IActionResult> RemoveContextWrapper(int contextId)
        {
            if (!HasAccess(_validationRepo.GetContextOwnerId(contextId)))
                return StatusCode(403);

            var ctx = await _patternRepo.GetContextAsync(contextId);
            if (await _patternRepo.DeleteContextAsync(ctx))
                return Ok();
            else
                return BadRequest();
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
