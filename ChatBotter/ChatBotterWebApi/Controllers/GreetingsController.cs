using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Greetings")]
    public class GreetingsController : Controller
    {
        private ChatBotContext _dbContext;

        public GreetingsController(ChatBotContext dbContext){
            this._dbContext = dbContext;
        }

        [Route("GetAllProjectGreetings")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProjectGreetings(int projectId){
            var res = await _dbContext.Greetings.Where(g => g.ProjectId == projectId).ToListAsync();
            return Ok(res);
        }

        [Route("GetGreeting")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGreeting(int id, int projectId)
        {
            var res = await _dbContext.Greetings.FirstOrDefaultAsync(g => g.Id == id);
            if (res != null)
                return Ok(res);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetRandomGreeting")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomGreeting(int projectId){
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
            if (!VerifyUser(_dbContext.TheProjects.Where(p => p.Id == projectId).First().OwnerId))
                return BadRequest();

            try

            {
                _dbContext.Greetings.Add(greeting);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex){
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("RemoveGreeting")]
        public async Task<IActionResult>  RemoveGreeting(int greetingId){
            var gr =  await _dbContext.Greetings.Where(g => g.Id == greetingId).FirstOrDefaultAsync();
            if (gr == null)
                return NotFound();
            else{
                _dbContext.Greetings.Remove(gr);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPost]
        [Route("UpdateGreeting")]
        public async Task<IActionResult> UpdateGreeting(Greeting greeting)
        {
            var gr = await _dbContext.Greetings.Where(g => g.Id == greeting.Id).FirstOrDefaultAsync();
            if (gr == null)
                return NotFound();
            else{
                gr.MainGreeting = greeting.MainGreeting;
                await _dbContext.SaveChangesAsync();
                return Ok();
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