using CBLib.Entities;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBLib
{
    public class ConcretePatternsRepository : IPatternsRepository
    {
        ChatBotContext _chatBotContext;

        public ConcretePatternsRepository(ChatBotContext chatBotContext){
            this._chatBotContext = chatBotContext;
        }

        /// <summary>
        /// All indexes of APP. key is a project id.
        /// </summary>
        private Dictionary<int, ContextsIndex> _indexes;

        public override void Init()
        {
            _indexes = new Dictionary<int, ContextsIndex>();
        }

        public override bool AddContext(ContextWrapper context)
        {
            try
            {
                _chatBotContext.Contexts.Add(context);
                if (_indexes.Where(i => i.Key == context.ProjectId).First().Value.AddContext(context)){
                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public override bool DeleteContext(ContextWrapper context)
        {
            if (_indexes.Where(i => i.Key == context.ProjectId).First().Value.DeleteContext(context))
                return true;

            return false;
        }

        public override bool UpdateContext(ContextWrapper context)
        {
            if(_indexes.Where(i => i.Key == context.ProjectId).First().Value.UpdateContext(context))
                return true;

            return false; ;
        }

        public override List<Context> GetActualContexts(int prjId, string[] terms)
        {
            throw new NotImplementedException();
        }

        public override void AddBotResponseToPattern(ContextWrapper context, BotResponse botResponse)
        {
            throw new NotImplementedException();
        }

        public override void DeleteBotResponseToPattern(ContextWrapper context, BotResponse botResponse)
        {
            throw new NotImplementedException();
        }

        public override List<BotResponse> GetBotResponsesToPattern(ContextWrapper context)
        {
            throw new NotImplementedException();
        }

        public override void UpdateBotResponseToPattern(ContextWrapper context, BotResponse botResponse)
        {
            throw new NotImplementedException();
        }

        public override bool CreateContextWithResponses(out ContextWrapper context, out List<BotResponse> responses)
        {
            throw new NotImplementedException();
        }

        public override bool AddManyContexts(List<ContextWrapper> contexts)
        {
            throw new NotImplementedException();
        }
    }
}
