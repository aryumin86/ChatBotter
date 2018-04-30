using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
using ChatBotterWebApi.Data;
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
    public class GreetingsController : Controller
    {
        private ChatBotContext _dbContext;
        private readonly ILogger _logger;
        private readonly IGreetingsRepository _greetingsRepo;

        public GreetingsController(ChatBotContext dbContext, ILogger<GreetingsController> logger, IGreetingsRepository greetingsRepo)
        {
            _dbContext = dbContext;
            _logger = logger;
            _greetingsRepo = greetingsRepo;
        }

        [Route("GetAllProjectGreetings/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProjectGreetings(int projectId){

            _logger.LogInformation("GetAllProjectGreetings({projectId}). All project greetings were requested", projectId);
            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!VerifyUser(prj.OwnerId))
                return StatusCode(403);

            //var res = await _dbContext.Greetings.Where(g => g.ProjectId == projectId).ToListAsync();  

            var res = await _greetingsRepo.GetAllProjectGreetingsAsync(projectId);
            return Ok(res);
        }

        [Route("GetAllAppGreetings")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAppGreetings()
        {
            _logger.LogInformation("GetAllAppGreetings(). All app greetings were requested");
            var res = await _greetingsRepo.GetAllAppGreetingsAsync();
            return Ok(res);
        }

        [Route("GetGreeting")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGreeting(int id, int projectId)
        {
            _logger.LogInformation("Greeting with id ({id}) was requested", id);

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!VerifyUser(prj.OwnerId))
                return StatusCode(403);

            var res = await _dbContext.Greetings.FirstOrDefaultAsync(g => g.Id == id);

            if (res != null)
                return Ok(res);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetRandomGreeting/{projectId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomGreeting(int projectId){

            _logger.LogInformation("GetRandomGreeting({projectId}). Random greeting was requested", projectId);

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!VerifyUser(prj.OwnerId))
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
        public async Task<IActionResult> AddGreeting(Greeting greeting, int projectId){
            _logger.LogInformation("AddGreeting({greeting},{projectId}). Adding greeting", greeting, projectId);

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return NotFound("There is no such project in db");

            if (!VerifyUser(prj.OwnerId))
                return StatusCode(403);

            try {
                _dbContext.Greetings.Add(greeting);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex){
                _logger.LogError(ex, "Cant't add greeting to project ({projectId})", projectId);
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
            if (!VerifyUser(prj.OwnerId))
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

            var gr = await _dbContext.Greetings.Where(g => g.Id == greeting.Id).FirstOrDefaultAsync();
            if (gr == null)
                return NotFound();

            var prj = _dbContext.TheProjects.FirstOrDefault(p => p.Id == gr.ProjectId);
            if (!VerifyUser(prj.OwnerId))
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
        private bool VerifyUser(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}