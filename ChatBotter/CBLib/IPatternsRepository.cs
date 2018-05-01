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

        bool AddContext(ContextWrapper context);

        bool DeleteContext(ContextWrapper context);

        bool UpdateContext(ContextWrapper context);

        List<Context> GetActualContexts(int prjId, string[] terms);

        bool AddBotResponseToPattern(BotResponse botResponse);

        bool DeleteBotResponseToPattern(BotResponse botResponse);

        bool UpdateBotResponseToPattern(BotResponse botResponse);

        List<BotResponse> GetBotResponsesToPattern(int contextId);

        bool CreateContextWithResponses(ContextWrapper context, IEnumerable<BotResponse> responses);

        bool AddManyContexts(List<ContextWrapper> contexts);

        /// <summary>
        /// Get response of bot for user message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<string> GetReponseToUserMessage(UserMessage message);
    }
}
