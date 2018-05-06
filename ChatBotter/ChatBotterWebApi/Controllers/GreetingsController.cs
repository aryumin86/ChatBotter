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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Greetings")]
    public class GreetingsController : Controller, IAccessVerifier
    {
        private ChatBotContext _dbContext;
        private readonly ILogger _logger;
        private readonly IGreetingsRepository _greetingsRepo;
        private readonly IMapper _mapper;

        public GreetingsController(ChatBotContext dbContext, ILogger<GreetingsController> logger, 
            IGreetingsRepository greetingsRepo, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _greetingsRepo = greetingsRepo;
            _mapper = mapper;
        }

        [Route("GetAllProjectGreetings/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllProjectGreetings(int projectId)
        {
            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            //var res = await _dbContext.Greetings.Where(g => g.ProjectId == projectId).ToListAsync();  

            var res = await _greetingsRepo.GetAllProjectGreetingsAsync(projectId);
            return Ok(res);
        }

        [Route("GetAllAppGreetings")]
        [HttpGet]
        public async Task<IActionResult> GetAllAppGreetings()
        {
            var res = await _greetingsRepo.GetAllAppGreetingsAsync();
            return Ok(res);
        }

        [Route("GetGreeting")]
        [HttpGet]
        public async Task<IActionResult> GetGreeting(int id, int projectId)
        {
            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            var res = await _dbContext.Greetings.FirstOrDefaultAsync(g => g.Id == id);

            if (res != null)
                return Ok(res);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetRandomGreeting/{projectId")]
        public async Task<IActionResult> GetRandomGreeting(int projectId)
        {
            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            Random rand = new Random();
            var prjGreetings = await _dbContext.Greetings.Where(g => g.ProjectId == projectId).ToListAsync();

            if (prjGreetings.Count() > 0)
                return Ok(prjGreetings.ToArray()[rand.Next(prjGreetings.Count() - 1)]);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("AddGreeting")]
        public async Task<IActionResult> AddGreeting(GreetingDto greeting){
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(greeting, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(greeting, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == greeting.ProjectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            try {
                var greetingObj = _mapper.Map<Greeting>(greeting);
                _dbContext.Greetings.Add(greetingObj);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex){
                _logger.LogError(ex, "Cant't add greeting to project ({projectId})", greeting.ProjectId);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("RemoveGreeting/{greetingId")]
        public async Task<IActionResult> RemoveGreeting(int greetingId){
            _logger.LogInformation("RemoveGreeting({greetingId}). Removing greeting", greetingId);

            var gr = await _dbContext.Greetings.Where(g => g.Id == greetingId).FirstOrDefaultAsync();
            if (gr == null)
                return NotFound();

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == gr.ProjectId);
            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);
            try
            {
                _dbContext.Greetings.Remove(gr);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Cant't remove greeting with ID ({greetingId})", greetingId);
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Route("UpdateGreeting")]
        public async Task<IActionResult> UpdateGreeting(Greeting greeting)
        {
            _logger.LogInformation("UpdateGreeting({greeting.Id}). Updating greeting", greeting.Id);

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(greeting, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(greeting, validationContext, validationResults, true);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gr = await _dbContext.Greetings.Where(g => g.Id == greeting.Id).FirstOrDefaultAsync();
            if (gr == null)
                return NotFound();

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == gr.ProjectId);
            if (!HasAccess(prj.OwnerId))
                return StatusCode(403);

            try
            {
                _dbContext.Entry(gr).CurrentValues.SetValues(greeting);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Cant't update greeting with ID ({greeting.Id})", greeting.Id);
                return BadRequest();
            }
        }

        /// <summary>
        /// Check if user has rights for the action
        /// (compares id's of 2 users -  current user and id of user as method parameter).
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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