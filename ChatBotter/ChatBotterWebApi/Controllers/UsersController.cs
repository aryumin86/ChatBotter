using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CBLib;
using ChatBotterWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ChatBotterWebApi.DTO;
using System.Security.Claims;
using ChatBotterWebApi.Helpers;

namespace ChatBotterWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller, IAccessVerifier
    {
        private readonly ChatBotContext _context;

        private readonly IMapper _mapper;

        public UsersController(ChatBotContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        public bool HasAccess(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userFromRepo == null)
                return NotFound($"Could not find the user with id = {id}");

            if (currentUserId != userFromRepo.Id)
                return Unauthorized();

            _mapper.Map(userForUpdateDto, userFromRepo);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
