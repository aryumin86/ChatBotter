using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CBLib;
using CBLib.Entities;
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
    [Route("api/TheProjects")]
    public class TheProjectsController : Controller, IAccessVerifier
    {
        private ChatBotContext _dbContext;
        private readonly ILogger _logger;
        private readonly IPatternsRepository _patternRepo;
        private readonly IMapper _mapper;

        public TheProjectsController(ChatBotContext ctx, ILogger<TheProjectsController> logger, IPatternsRepository patternRepo, IMapper mapper)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetProject/")]
        public async Task<IActionResult> GetProject(int prjId)
        {
            var res = await _dbContext.TheProjects.FirstOrDefaultAsync(p => p.Id == prjId);

            if (res == null)
                return NotFound("No such project found");

            if(!HasAccess(res.OwnerId))
                return StatusCode(403);

            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllCurrentUserProjects")]
        public async Task<IActionResult> GetAllCurrentUserProjects()
        {
            return Ok(await _dbContext.TheProjects.FirstOrDefaultAsync(p => p.OwnerId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }

        [HttpGet]
        [Route("GetAllAppProjects")]
        public IActionResult GetAllAppProjects()
        {
            if(!_dbContext.Users.First(u => u.Id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)).AppAdmin)
                return StatusCode(403);

            return Ok(_dbContext.TheProjects);
        }

        [HttpPost]
        [Route("UpdateProject")]
        public async Task<IActionResult> UpdateProject([FromBody] TheProjectDto prj)
        {
            if(!HasAccess(prj.OwnerId))
                return StatusCode(403);

            var prjFromDb = _dbContext.TheProjects.FirstOrDefault(p => p.Id == prj.Id);

            if (prjFromDb == null)
                return NotFound("No such project in database");

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(prj, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(prj, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var prjObj = _mapper.Map<TheProject>(prj);
                _dbContext.Entry(prjFromDb).CurrentValues.SetValues(prjObj);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Cant't update project ({prj.Id})", prj.Id);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("RemoveProject/{prjId}")]
        public async Task<IActionResult> RemoveProject(int prjId)
        {
            var prj = await _dbContext.TheProjects.FirstOrDefaultAsync(p => p.Id == prjId);

            if (prj == null)
                return NotFound("No such project in database");

            if(!HasAccess(prj.OwnerId))
                return StatusCode(403);

            try
            {
                _dbContext.TheProjects.Remove(prj);
                await _dbContext.SaveChangesAsync();
                _patternRepo.OnProjectDeleted();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Cant't remove project with ID ({prjId})", prjId);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddProject")]
        public async Task<IActionResult> AddProject([FromBody] TheProjectDto prj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(prj, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(prj, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var prjObj = _mapper.Map<TheProject>(prj);
                prjObj.CreatedAt = DateTime.Now;

                _dbContext.TheProjects.Add(prjObj);
                await _dbContext.SaveChangesAsync();
                _patternRepo.OnProjectDeleted();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant't add project");
                return BadRequest();
            }
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
