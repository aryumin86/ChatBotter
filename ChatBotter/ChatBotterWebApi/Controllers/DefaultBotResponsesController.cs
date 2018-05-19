using AutoMapper;
using CBLib;
using CBLib.Entities;
using ChatBotterWebApi.Data;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/DefaultBotResponses")]
    public class DefaultBotResponsesController : Controller, IAccessVerifier
    {
        private ChatBotContext _dbContext;
        private IDefaultBotResponsesRepository _defRespRepo;
        private readonly IMapper _mapper;
        private readonly IValidationHelperRepository _validationRepo;

        public DefaultBotResponsesController(ChatBotContext ctx,
            IDefaultBotResponsesRepository defRespRepo, IMapper mapper, IValidationHelperRepository validationRepo)
        {
            _dbContext = ctx;
            _defRespRepo = defRespRepo;
            _mapper = mapper;
            _validationRepo = validationRepo;
        }

        [HttpGet(Name = "id")]
        [Route("GetDefaultBotResponse")]
        public async Task<IActionResult> GetDefaultBotResponse(int id)
        {
            var resp = await _defRespRepo.GetDefaultBotResponseAsync(id);
            if (resp == null)
                return NotFound();

            if(!HasAccess(_validationRepo.GetDefaultResponseOwnerId(id)))
                return StatusCode(403);

            return Ok(resp);
        }

        [HttpGet("{prjId}")]
        [Route("GetAllProjectBotResponses")]
        public async Task<IActionResult> GetAllProjectBotResponses(int prjId)
        {
            if (!HasAccess(_validationRepo.GetProjectOwnerId(prjId)))
                return StatusCode(403);

            var res = await _defRespRepo.GetAllProjectBotResponsesAsync(prjId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllAppBotResponses")]
        public async Task<IActionResult> GetAllAppBotResponses()
        {
            if (!HasAccess(-1))
                return StatusCode(403);

            var res = await _defRespRepo.GetAllAppBotResponsesAsync();
            return Ok(res);
        }

        [HttpPost]
        [Route("AddDefaultBotResponseAsync")]
        public async Task<IActionResult> AddDefaultBotResponseAsync([FromBody]DefaultBotResponse resp)
        {
            if (resp == null)
                return BadRequest();

            if (!HasAccess(_validationRepo.GetProjectOwnerId(resp.TheProjectId)))
                return StatusCode(403);

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(resp, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(resp, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _defRespRepo.AddDefaultBotResponseAsync(resp))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("UpdateDefaultBotResponseAsync")]
        public async Task<IActionResult> UpdateDefaultBotResponseAsync([FromBody]DefaultBotResponse resp)
        {
            if (resp == null)
                return BadRequest();

            if (!HasAccess(_validationRepo.GetProjectOwnerId(resp.TheProjectId)))
                return StatusCode(403);

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(resp, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(resp, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _defRespRepo.UpdateDefaultBotResponseAsync(resp))
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet("id")]
        [Route("DeleteDefaultBotResponseAsync")]
        public async Task<IActionResult> DeleteDefaultBotResponseAsync(int id)
        {
            var resp = await _defRespRepo.GetDefaultBotResponseAsync(id);
            if (resp == null)
                return NotFound();

            if (!HasAccess(_validationRepo.GetDefaultResponseOwnerId(id)))
                return StatusCode(403);

            if(await _defRespRepo.DeleteDefaultBotResponseAsync(id))
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
