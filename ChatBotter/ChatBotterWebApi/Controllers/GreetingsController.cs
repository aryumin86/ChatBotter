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
        [Route("GetAllProjectGreetings")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllProjectGreetings(int projectId){
            using(var db = new ChatBotContext()){
                return Ok(db.Greetings.Where(g => g.ProjectId == projectId));
            }
        }

        [Route("GetGreeting")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetGreeting(int id, int projectId)
        {
            using (var db = new ChatBotContext())
            {
                var res = db.Greetings.Where(g => g.Id == id).FirstOrDefault();
                if (res != null)
                    return Ok(res);
                else
                    return NotFound();
            }
        }

        [HttpGet]
        [Route("GetRandomGreeting")]
        [AllowAnonymous]
        public IActionResult GetRandomGreeting(int projectId){
            Random rand = new Random();
            using (var db = new ChatBotContext())
            {
                var prjGreetings = db.Greetings.Where(g => g.ProjectId == projectId);

                if (prjGreetings.Count() > 0)
                    return Ok(prjGreetings.ToArray()[rand.Next(prjGreetings.Count() - 1)]);
                else
                    return NotFound();
            }
        }

        [HttpPost]
        [Route("AddGreeting")]
        public void AddGreeting(Greeting greeting, int projectId){
            using(var db = new ChatBotContext()){
                db.Greetings.Add(greeting);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("RemoveGreeting")]
        public IActionResult RemoveGreeting(int greetingId){
            using(var db = new ChatBotContext()){
                var gr = db.Greetings.Where(g => g.Id == greetingId).FirstOrDefault();
                if (gr == null)
                    return NotFound();
                else{
                    db.Greetings.Remove(gr);
                    db.SaveChanges();
                    return Ok();
                } 
            }
        }

        [HttpPost]
        [Route("UpdateGreeting")]
        public IActionResult UpdateGreeting(Greeting greeting)
        {
            using (var db = new ChatBotContext()){
                var gr = db.Greetings.Where(g => g.Id == greeting.Id).FirstOrDefault();
                if (gr == null)
                    return NotFound();
                else{
                    gr.MainGreeting = greeting.MainGreeting;
                    db.SaveChanges();
                    return Ok();
                }
            }
        }
    }
}