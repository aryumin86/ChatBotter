using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotterWebApi.Controllers
{
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
        public IActionResult GetAllProjectGreetings(int projectId){
            return Ok(_dbContext.Greetings.Where(g => g.ProjectId == projectId));
        }

        [Route("GetGreeting")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetGreeting(int id, int projectId)
        {
            var res = _dbContext.Greetings.Where(g => g.Id == id).FirstOrDefault();
            if (res != null)
                return Ok(res);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetRandomGreeting")]
        [AllowAnonymous]
        public IActionResult GetRandomGreeting(int projectId){
            Random rand = new Random();
            var prjGreetings = _dbContext.Greetings.Where(g => g.ProjectId == projectId);

            if (prjGreetings.Count() > 0)
                return Ok(prjGreetings.ToArray()[rand.Next(prjGreetings.Count() - 1)]);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("AddGreeting")]
        public void AddGreeting(Greeting greeting, int projectId){
            _dbContext.Greetings.Add(greeting);
            _dbContext.SaveChanges();
        }

        [HttpGet]
        [Route("RemoveGreeting")]
        public IActionResult RemoveGreeting(int greetingId){
            var gr = _dbContext.Greetings.Where(g => g.Id == greetingId).FirstOrDefault();
            if (gr == null)
                return NotFound();
            else{
                _dbContext.Greetings.Remove(gr);
                _dbContext.SaveChanges();
                return Ok();
            }
        }

        [HttpPost]
        [Route("UpdateGreeting")]
        public IActionResult UpdateGreeting(Greeting greeting)
        {
            var gr = _dbContext.Greetings.Where(g => g.Id == greeting.Id).FirstOrDefault();
            if (gr == null)
                return NotFound();
            else{
                gr.MainGreeting = greeting.MainGreeting;
                _dbContext.SaveChanges();
                return Ok();
            }
        }
    }
}