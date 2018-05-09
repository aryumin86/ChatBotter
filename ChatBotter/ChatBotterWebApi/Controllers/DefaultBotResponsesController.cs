using CBLib;
using ChatBotterWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Controllers
{
    public class DefaultBotResponsesController : Controller, IAccessVerifier
    {
        public Task<IActionResult> GetDefaultBotResponse()
        {

        }

        public Task<IActionResult> GetAllProjectBotResponses()
        {

        }

        public Task<IActionResult> GetAllAppBotResponses()
        {

        }

        public Task<IActionResult> AddDefaultBotResponseAsync()
        {

        }

        public Task<IActionResult> UpdateDefaultBotResponseAsync()
        {

        }

        public Task<IActionResult> DeleteDefaultBotResponseAsync()
        {

        }



        public bool HasAccess(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
