using CBLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Data
{
    public interface IDefaultBotResponsesRepository
    {
        Task<DefaultBotResponse> GetDefaultBotResponseAsync(int id);
        Task<IEnumerable<DefaultBotResponse>> GetAllProjectBotResponsesAsync(int prjId);
        Task<IEnumerable<DefaultBotResponse>> GetAllAppBotResponsesAsync();
        Task<bool> AddDefaultBotResponseAsync(DefaultBotResponse resp);
        Task<bool> UpdateDefaultBotResponseAsync(DefaultBotResponse resp);
        Task<bool> DeleteDefaultBotResponseAsync(int id);
    }
}
