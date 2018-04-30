using CBLib.Entities;
using Microsoft.Extensions.Logging;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBLib
{
    /// <summary>
    /// Simple implementation of pattern repository without index of words etc.
    /// </summary>
    public class ConcretePatternsRepository : IPatternsRepository
    {
        private ChatBotContext _chatBotContext;
        private ILogger _logger;
        private ConcreteMorfDictionary _morfDict;
        private ConcretePatternParser _parser;

        /// <summary>
        /// All projects contexts. Key is a project Id and value - List of contexts.
        /// </summary>
        private Dictionary<int, List<ContextWrapper>> _projectsContexts;
        /// <summary>
        /// All reponses of all projects. Key - context id, value - BotResponses of this context.
        /// </summary>
        private Dictionary<int, List<BotResponse>> _contextsResponses;


        public ConcretePatternsRepository(ChatBotContext chatBotContext, ILogger logger){
            _chatBotContext = chatBotContext;
            _logger = logger;
            _morfDict = new ConcreteMorfDictionary();
            _parser = new ConcretePatternParser();
        }        

        public void Init()
        {
            var allContexts = _chatBotContext.Contexts
                .GroupBy(g => g.ProjectId)
                .Select(g => new
                {
                    PrjId = g.Key,
                    Contexts = g
                })
                .ToDictionary(g => g.PrjId, g => g.Contexts.OrderBy(c => c.Priority));

            //initiating contexts objects
            foreach(var pKey in allContexts.Keys)
            {
                foreach (var ctx in allContexts[pKey])
                    ctx.InitExistingContext(_parser);
            }

            _contextsResponses = _chatBotContext.BotResponses
                .GroupBy(r => r.PatternId)
                .Select(gr => new
                {
                    CtxId = gr.Key,
                    Responses = gr
                })
                .ToDictionary(r => r.CtxId, r => r.Responses.OrderBy(x => x.Priority).ToList());
        }

        public bool AddBotResponseToPattern(BotResponse botResponse)
        {
            try
            {
                _chatBotContext.BotResponses.Add(botResponse);
                _chatBotContext.SaveChanges();
                _contextsResponses[botResponse.PatternId].Add(botResponse);

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't add response to pattern");
                return false;
            }
        }

        public bool AddContext(ContextWrapper context)
        {
            try
            {
                _chatBotContext.Contexts.Add(context);
                _chatBotContext.SaveChanges();
                _projectsContexts[context.ProjectId].Add(context);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't add context");
                return false;
            }
        }

        public bool AddManyContexts(List<ContextWrapper> contexts)
        {
            foreach(var context in contexts)
            {
                _chatBotContext.Contexts.Add(context);
                _projectsContexts[context.ProjectId].Add(context);
            }

            try
            {
                _chatBotContext.SaveChanges();
                Init();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't add contexts");
                return false;
            }
        }

        public bool CreateContextWithResponses(ContextWrapper context, IEnumerable<BotResponse> responses)
        {
            try
            {
                _chatBotContext.Contexts.Add(context);
                _chatBotContext.SaveChanges();
                _projectsContexts[context.ProjectId].Add(context);

                for (int i = 0; i < responses.Count(); i++)
                {
                    responses.ElementAt(i).Pattern = context;
                    responses.ElementAt(i).PatternId = context.Id;
                    _chatBotContext.BotResponses.Add(responses.ElementAt(i));
                    _contextsResponses[responses.ElementAt(i).PatternId].Add(responses.ElementAt(i));
                }

                _chatBotContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't add context with responses");
                return false;
            }
        }

        public bool DeleteBotResponseToPattern(BotResponse botResponse)
        {
            try
            {
                _chatBotContext.BotResponses.Remove(botResponse);
                _chatBotContext.SaveChanges();
                var respToRemove = _contextsResponses[botResponse.PatternId]
                    .Where(r => r.Id == botResponse.Id).First();
                _contextsResponses[botResponse.PatternId].Remove(respToRemove);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't remove response");
                return false;
            }
        }

        public bool DeleteContext(ContextWrapper context)
        {
            try
            {
                _chatBotContext.Contexts.Remove(context);
                _chatBotContext.SaveChanges();
                var contextToRemove = _projectsContexts[context.ProjectId]
                    .Where(ctx => ctx.Id == context.Id).First();
                _projectsContexts[context.ProjectId].Remove(contextToRemove);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't remove context");
                return false;
            }
        }

        public List<Context> GetActualContexts(int prjId, string[] terms)
        {
            throw new NotImplementedException();
        }

        public List<BotResponse> GetBotResponsesToPattern(int contextId)
        {
            return _contextsResponses[contextId];
        }

        /// <summary>
        /// Most important method!!!!
        /// </summary>
        /// <param name="message"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool GetReponseToUserMessage(string message, int projectId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBotResponseToPattern(BotResponse botResponse)
        {
            try
            {
                var respFromDb = _chatBotContext.BotResponses.First(r => r.Id == botResponse.Id);
                _chatBotContext.Entry(respFromDb).CurrentValues.SetValues(botResponse);
                _chatBotContext.SaveChanges();
                _contextsResponses[botResponse.PatternId]
                    .Where(r => r.Id == botResponse.Id)
                    .First() = botResponse;

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't update response");
                return false;
            }
        }

        public bool UpdateContext(ContextWrapper context)
        {
            try
            {
                var ctxFromDb = _chatBotContext.Contexts.First(r => r.Id == context.Id);
                _chatBotContext.Entry(ctxFromDb).CurrentValues.SetValues(context);
                _chatBotContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't update context");
                return false;
            }
        }
    }
}
