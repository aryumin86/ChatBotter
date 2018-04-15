using System;
using ChatBotterWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChatBotterWebApi.Controllers
{
    public class RegController : Controller
    {
        private IAuthRepository _repo;
        private IConfiguration _config { get; }

        public RegController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
    }
}
