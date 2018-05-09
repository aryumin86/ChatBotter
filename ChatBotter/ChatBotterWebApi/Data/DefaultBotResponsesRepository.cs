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
    public class DefaultBotResponsesRepository : IDefaultBotResponsesRepository
    {
        private ChatBotContext _dbContext;
        private readonly ILogger _logger;

        public DefaultBotResponsesRepository(ChatBotContext dbContext, ILogger<DefaultBotResponsesRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddDefaultBotResponseAsync(DefaultBotResponse resp)
        {
            try
            {
                await _dbContext.DefaultBotResponses.AddAsync(resp);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Default bot response ({resp.Id}) was added", resp.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't add default bot response");
                return false;
            }
        }

        public async Task<bool> DeleteDefaultBotResponseAsync(int id)
        {
            try
            {
                var resp = await _dbContext.DefaultBotResponses.FindAsync(id);
                _dbContext.DefaultBotResponses.Remove(resp);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Default bot response ({id}) was deleted", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't delete default bot response ({id})", id);
                return false;
            }
        }

        public async Task<IEnumerable<DefaultBotResponse>> GetAllAppBotResponsesAsync(int prjId)
        {
            return await _dbContext.DefaultBotResponses.ToListAsync();
        }

        public async Task<IEnumerable<DefaultBotResponse>> GetAllProjectBotResponsesAsync(int prjId)
        {
            return await _dbContext.DefaultBotResponses.Where(r => r.TheProjectId == prjId).ToListAsync();
        }

        public async Task<DefaultBotResponse> GetDefaultBotResponseAsync(int id)
        {
            return await _dbContext.DefaultBotResponses.FindAsync(id);
        }

        public async Task<bool> UpdateDefaultBotResponseAsync(DefaultBotResponse resp)
        {
            try
            {
                var respInDb = await _dbContext.DefaultBotResponses.FindAsync(resp.Id);
                _dbContext.Entry(respInDb).CurrentValues.SetValues(resp);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Default bot response ({resp.Id}) was updated", resp.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't update default bot response ({resp.Id})", resp.Id);
                return false;
            }
        }
    }
}
