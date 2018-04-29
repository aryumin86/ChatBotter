using CBLib.Entities;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Text;

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

        void AddBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        void DeleteBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        void UpdateBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        List<BotResponse> GetBotResponsesToPattern(ContextWrapper context);

        bool CreateContextWithResponses(out ContextWrapper context, out List<BotResponse> responses);

        bool AddManyContexts(List<ContextWrapper> contexts);
    }
}
