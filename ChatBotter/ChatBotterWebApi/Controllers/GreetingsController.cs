using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotterWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Greetings")]
    public class GreetingsController : Controller
    {
        [Route("get")]
        public IActionResult Get()
        {
            using (var db = new ChatBotContext())
            {
                var res = db.Greetings.ToList();
                return Ok(res);
            }
        }
    }
}