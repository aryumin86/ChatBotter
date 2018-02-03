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
        protected List<Context> _contexts;

        /// <summary>
        /// Gets all patterns from db and adds them to contexts index.
        /// </summary>
        public abstract void Init();

        public abstract bool AddContext(Context context);

        public abstract bool DeleteContext(Context context);

        public abstract bool UpdateContext(Context context);

        public abstract List<Context> GetActualContexts(int prjId, string[] terms);

        public abstract void AddBotResponseToPattern(Context context, BotResponse botResponse);

        public abstract void DeleteBotResponseToPattern(Context context, BotResponse botResponse);

        public abstract void UpdateBotResponseToPattern(Context context, BotResponse botResponse);

        public abstract List<BotResponse> GetBotResponsesToPattern(Context context);
    }
}
