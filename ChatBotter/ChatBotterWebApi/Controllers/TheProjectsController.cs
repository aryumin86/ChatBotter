using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
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

        public TheProjectsController(ChatBotContext ctx, ILogger logger, IPatternsRepository patternRepo)
        {
            _dbContext = ctx;
            _logger = logger;
            _patternRepo = patternRepo;
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
        public async Task<IActionResult> UpdateProject([FromBody] TheProject prj)
        {
            if(!HasAccess(prj.OwnerId))
                return StatusCode(403);

            var prjFromDb = _dbContext.TheProjects.FirstOrDefault(p => p.Id == prj.Id);

            if (prjFromDb == null)
                return NotFound("No such project in database");

            try
            {
                _dbContext.Entry(prjFromDb).CurrentValues.SetValues(prj);
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
        public async Task<IActionResult> AddProject([FromBody] TheProject prj)
        {
            try
            {
                _dbContext.TheProjects.Add(prj);
                await _dbContext.SaveChangesAsync();
                _patternRepo.OnProjectDeleted();
                return Ok();
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
