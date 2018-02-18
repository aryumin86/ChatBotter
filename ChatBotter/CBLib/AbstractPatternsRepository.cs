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
    public abstract class AbstractPatternsRepository
    {
        protected ChatBotContext _chatBotContext;

        public AbstractPatternsRepository(ChatBotContext chatBotContext){
            this._chatBotContext = chatBotContext;
        }

        

        /// <summary>
        /// Gets all patterns from db and adds them to contexts index.
        /// </summary>
        public abstract void Init();

        public abstract bool AddContext(ContextWrapper context);

        public abstract bool DeleteContext(ContextWrapper context);

        public abstract bool UpdateContext(ContextWrapper context);

        public abstract List<Context> GetActualContexts(int prjId, string[] terms);

        public abstract void AddBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        public abstract void DeleteBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        public abstract void UpdateBotResponseToPattern(ContextWrapper context, BotResponse botResponse);

        public abstract List<BotResponse> GetBotResponsesToPattern(ContextWrapper context);

        public abstract bool CreateContextWithResponses(out ContextWrapper context, out List<BotResponse> responses);
    }
}
