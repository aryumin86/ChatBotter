using CBLib.Entities;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBLib
{
    /// <summary>
    /// All patterns for all projects and actions with then
    /// including fast search of probable success matching with
    /// string patterns.
    /// </summary>
    public interface IPatternsRepository
    {
        /// <summary>
        /// Gets all patterns from db and adds them to contexts index.
        /// </summary>
        void Init();

        Task<ContextWrapper> GetContextAsync(int contextId);

        Task<IEnumerable<ContextWrapper>> GetAllProjectContextsAsync(int projectId);

        Task AddContextAsync(ContextWrapper context);

        Task DeleteContextAsync(ContextWrapper context);

        Task UpdateContextAsync(ContextWrapper context);

        Task<IEnumerable<Context>> GetActualContextsAsync(int prjId, string[] terms);

        Task AddBotResponseToPatternAsync(BotResponse botResponse);

        Task DeleteBotResponseToPatternAsync(BotResponse botResponse);

        Task UpdateBotResponseToPatternAsync(BotResponse botResponse);

        IEnumerable<BotResponse> GetBotResponsesToPatternAsync(int contextId);

        Task CreateContextWithResponsesAsync(ContextWrapper context, IEnumerable<BotResponse> responses);

        Task AddManyContextsAsync(List<ContextWrapper> contexts);

        /// <summary>
        /// Get response of bot for user message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        BotResponse GetReponseToUserMessageAsync(UserMessage message);
    }
}
