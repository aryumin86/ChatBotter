using CBLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private AbstractMorfDictionary _dict;

        /// <summary>
        /// All projects contexts. Key is a project Id and value - List of contexts.
        /// </summary>
        private Dictionary<int, List<ContextWrapper>> _projectsContexts;
        /// <summary>
        /// All reponses of all projects. Key - context id, value - BotResponses of this context.
        /// </summary>
        private Dictionary<int, List<BotResponse>> _contextsResponses;


        private ChatBotContext createDbContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("chatbotter_mysql_conn");

            var optionsBuilder = new DbContextOptionsBuilder<ChatBotContext>();
            var options = optionsBuilder
                .UseMySql(connectionString)
                .Options;

            ChatBotContext ctx = new ChatBotContext(options);
            return ctx;
        }


        public ConcretePatternsRepository(/*ChatBotContext chatBotContext, */ ILogger<ConcretePatternsRepository> logger){


            _chatBotContext = createDbContext();
            _logger = logger;
            _morfDict = new ConcreteMorfDictionary();
            _parser = new ConcretePatternParser();
            _dict = new ConcreteMorfDictionary();

            Init();
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
                .ToDictionary(r => r.CtxId, r => r.Responses.OrderBy(x => x.Priority)
                .ToList());

            _projectsContexts = _chatBotContext.Contexts
                .GroupBy(c => c.ProjectId)
                .Select(gr => new
                {
                    PrjId = gr.Key,
                    Contexts = gr
                })
                .ToDictionary(r => r.PrjId, r => r.Contexts.OrderBy(x => x.Priority)
                .ToList());
        }

        public async Task<bool> AddBotResponseToPatternAsync(BotResponse botResponse)
        {
            try
            {
                _chatBotContext.BotResponses.Add(botResponse);
                var res = await _chatBotContext.SaveChangesAsync();
                _contextsResponses[botResponse.PatternId].Add(botResponse);
                _logger.LogInformation("Bot response added ({botResponse.Id})", botResponse.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't add response to pattern");
                return false;
            }
        }

        public async Task<bool> AddContextAsync(ContextWrapper context)
        {
            try
            {
                context.InitNewContext(context.ExpressionRawStr, _parser, _dict, 
                    shouldWorkWithTermsForms: true, tokenFormsMaxNumberForAsterix: 30);

                _chatBotContext.Contexts.Add(context);
                await _chatBotContext.SaveChangesAsync();
                _projectsContexts[context.ProjectId].Add(context);
                _logger.LogInformation("Pattern added ({context.Id})", context.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't add context");
                return false;
            }
        }

        public async Task<bool> AddManyContextsAsync(List<ContextWrapper> contexts)
        {
            try
            {
                foreach (var context in contexts)
                {
                    context.InitNewContext(context.ExpressionRawStr, _parser, _dict,
                        shouldWorkWithTermsForms: true, tokenFormsMaxNumberForAsterix: 30);
                    _chatBotContext.Contexts.Add(context);
                    _projectsContexts[context.ProjectId].Add(context);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't init contexts");
            }            

            try
            {
                await _chatBotContext.SaveChangesAsync();
                Init();
                _logger.LogInformation("Patterns added");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't add contexts to db");
            }

            return false;
        }

        public async Task<bool> CreateContextWithResponsesAsync(ContextWrapper context, IEnumerable<BotResponse> responses)
        {
            try
            {
                context.InitNewContext(context.ExpressionRawStr, _parser, _dict,
                        shouldWorkWithTermsForms: true, tokenFormsMaxNumberForAsterix: 30);
                _chatBotContext.Contexts.Add(context);
                await _chatBotContext.SaveChangesAsync();
                _projectsContexts[context.ProjectId].Add(context);

                for (int i = 0; i < responses.Count(); i++)
                {
                    responses.ElementAt(i).Pattern = context;
                    responses.ElementAt(i).PatternId = context.Id;
                    _chatBotContext.BotResponses.Add(responses.ElementAt(i));
                    _contextsResponses[responses.ElementAt(i).PatternId].Add(responses.ElementAt(i));
                }

                await _chatBotContext.SaveChangesAsync();
                _logger.LogInformation("Contexts with responses added ({context.Id})", context.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't add context with responses");
                return false;
            }
        }

        public async Task<bool> DeleteBotResponseToPatternAsync(BotResponse botResponse)
        {
            try
            {
                _chatBotContext.BotResponses.Remove(botResponse);
                await _chatBotContext.SaveChangesAsync();
                var respToRemove = _contextsResponses[botResponse.PatternId]
                    .Where(r => r.Id == botResponse.Id).First();
                _contextsResponses[botResponse.PatternId].Remove(respToRemove);
                _logger.LogInformation("Bot response deleted ({botResponse.Id})", botResponse.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't remove response");
                return false;
            }
        }

        public async Task<bool> DeleteContextAsync(ContextWrapper context)
        {
            try
            {
                _chatBotContext.Contexts.Remove(context);
                await _chatBotContext.SaveChangesAsync();
                var contextToRemove = _projectsContexts[context.ProjectId]
                    .Where(ctx => ctx.Id == context.Id).First();
                _projectsContexts[context.ProjectId].Remove(contextToRemove);
                _logger.LogInformation("Pattern deleted ({context.Id})", context.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't remove context");
                return false;
            }
        }

        /// <summary>
        /// No need here. Simple implementation of repo. withot index.
        /// </summary>
        /// <param name="prjId"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public Task<IEnumerable<Context>> GetActualContextsAsync(int prjId, string[] terms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BotResponse> GetBotResponsesToPatternAsync(int contextId)
        {
            return _contextsResponses[contextId];
        }

        public BotResponse GetReponseToUserMessageAsync(UserMessage message)
        {
            var tpp = new TextsPreProcessor();
            var clearedTextAsTokens = tpp.SimpleUserMessagePrepocessing(message.MessageText);

            foreach (var pa in _projectsContexts[message.ProjectId])
            {
                if (pa.Ctx.MatchPatternToString(clearedTextAsTokens))
                    return _contextsResponses[pa.Id].OrderBy(r => r.Priority).First();
            }

            return null;
        }

        public async Task<bool> UpdateBotResponseToPatternAsync(BotResponse botResponse)
        {
            try
            {
                var respFromDb = _chatBotContext.BotResponses.First(r => r.Id == botResponse.Id);
                _chatBotContext.Entry(respFromDb).CurrentValues.SetValues(botResponse);
                await _chatBotContext.SaveChangesAsync();
                var ctxResps = _contextsResponses[botResponse.PatternId];
                var ind = ctxResps.FindIndex(r => r.Id == botResponse.Id);
                ctxResps[ind] = botResponse;
                _logger.LogInformation("Resonse updated ({botResponse.Id})", botResponse.Id);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "can't update response");
                return false;
            }
        }

        public async Task<bool> UpdateContextAsync(ContextWrapper context)
        {
            try
            {
                var ctxFromDb = _chatBotContext.Contexts.First(r => r.Id == context.Id);
                _chatBotContext.Entry(ctxFromDb).CurrentValues.SetValues(context);
                await _chatBotContext.SaveChangesAsync();

                var projectContexts = _projectsContexts[context.ProjectId];
                var indexOfContext = projectContexts.FindIndex(c => c.Id == context.Id);
                projectContexts[indexOfContext] = context;
                _logger.LogInformation("Pattern updated ({context.Id})", context.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't update context");
                return false;
            }
        }

        public Task<ContextWrapper> GetContextAsync(int contextId)
        {
            return Task.Run(() => _chatBotContext.Contexts.FindAsync(contextId));
            //return _chatBotContext.Contexts.First(c => c.Id == contextId);
        }

        public async Task<IEnumerable<ContextWrapper>> GetAllProjectContextsAsync(int projectId)
        {
            var res = await _chatBotContext.Contexts.Where(c => c.ProjectId == projectId).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<BotResponse>> GetAllProjectBotResponsesAsync(int projectId)
        {
            var res = await _chatBotContext.BotResponses.Where(r => r.TheProjectId == projectId).ToListAsync();
            return res;
        }

        public async Task<BotResponse> GetBotResponseAsync(int id)
        {
            var res = await _chatBotContext.BotResponses.FindAsync(id);
            return res;
        }

        public async Task<bool> RemoveBotResponseAsync(int id)
        {
            try
            {
                var resp = await _chatBotContext.BotResponses.FirstOrDefaultAsync(r => r.Id == id);
                if (resp != null)
                {
                    _chatBotContext.BotResponses.Remove(resp);
                    var respToRemove = _contextsResponses[resp.PatternId].FirstOrDefault(r => r.Id == id);
                    if (respToRemove != null)
                        _contextsResponses[resp.PatternId].Remove(respToRemove);
                    _logger.LogInformation("Response deleted ({id})", id);
                    return true;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't remove bot response");
            }

            return false;
        }

        public void OnProjectAdded()
        {
            Init();
        }

        public void OnProjectDeleted()
        {
            Init();
        }
    }
}
