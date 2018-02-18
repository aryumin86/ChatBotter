using CBLib.Entities;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBLib
{
    public class ConcretePatternsRepository : AbstractPatternsRepository
    {
        public ConcretePatternsRepository(ChatBotContext chatBotContext) : base(chatBotContext){
            
        }

        private ContextsIndex _index;
        private string _connectionString;
        //private Logger

        public override void Init()
        {
            _index = new ContextsIndex(_chatBotContext.Contexts);
        }

        public override bool AddContext(ContextWrapper context)
        {
            try
            {
                _chatBotContext.Contexts.Add(context);
                if (_index.AddContext(context)){
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
            _index.DeleteContext(context);
            throw new NotImplementedException();
        }

        public override bool UpdateContext(ContextWrapper context)
        {
            _index.UpdateContext(context);
            throw new NotImplementedException();
        }

        public override List<Context> GetActualContexts(int prjId, string[] terms)
        {
            return _index.GetContextsForTerms(terms, prjId);
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
    }
}
