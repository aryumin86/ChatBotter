using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBLib;
using CBLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatBotterWebApi.Data
{
    public class GreetingsRepository : IGreetingsRepository
    {
        private readonly ChatBotContext _context;
        private readonly ILogger _logger;

        public GreetingsRepository(ChatBotContext context, ILogger<GreetingsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Greeting AddGreetingAsync(Greeting greeting, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Greeting>> GetAllAppGreetingsAsync()
        {
            return await _context.Greetings.ToListAsync();
        }

        public async Task<IEnumerable<Greeting>> GetAllProjectGreetingsAsync(int projectId)
        {
            return await _context.Greetings.Where(g => g.ProjectId == projectId).ToListAsync();
        }

        public Greeting GetGreetingAsync(int id, int projectId)
        {
            throw new NotImplementedException();
        }

        public Greeting GetRandomGreetingAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGreetingAsync(int greetingId)
        {
            throw new NotImplementedException();
        }

        public Greeting UpdateGreetingAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }
    }
}
