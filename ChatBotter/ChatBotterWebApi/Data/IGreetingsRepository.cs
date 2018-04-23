using CBLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Data
{
    public interface IGreetingsRepository
    {
        Task<IEnumerable<Greeting>> GetAllProjectGreetingsAsync(int projectId);
        Task<IEnumerable<Greeting>> GetAllAppGreetingsAsync();
        Greeting GetGreetingAsync(int id, int projectId);
        Greeting GetRandomGreetingAsync(int projectId);
        Greeting AddGreetingAsync(Greeting greeting, int projectId);
        bool RemoveGreetingAsync(int greetingId);
        Greeting UpdateGreetingAsync(Greeting greeting);
    }
}
