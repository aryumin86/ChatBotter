using CBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Data
{
    public class ValidationHelperRepository : IValidationHelperRepository
    {
        private ChatBotContext _chatBotContext;

        public ValidationHelperRepository(ChatBotContext chatBotContext)
        {
            _chatBotContext = chatBotContext;
        }

        public int GetContextOwnerId(int contextId)
        {
            var ctx = _chatBotContext.Contexts.Find(contextId);
            var prj = _chatBotContext.TheProjects.Find(ctx.Ctx.ProjectId);
            return prj.OwnerId;
        }

        public int GetDefaultResponseOwnerId(int responseId)
        {
            var defResp = _chatBotContext.DefaultBotResponses.Find(responseId);
            if (defResp == null)
                return -1;
            var prj = defResp.TheProjectId;
            return _chatBotContext.TheProjects.Find(prj).OwnerId;
        }

        public int GetFarewellOwnerId(int farewellId)
        {
            var farewell = _chatBotContext.Farewells.Find(farewellId);
            var prj = _chatBotContext.TheProjects.Find(farewell.ProjectId);
            return prj.OwnerId;
        }

        public int GetGreetingOwnerId(int greetingId)
        {
            var greeting = _chatBotContext.Greetings.Find(greetingId);
            var prj = _chatBotContext.TheProjects.Find(greeting.ProjectId);
            return prj.OwnerId;
        }

        public int GetProjectOwnerId(int projectId)
        {
            var prj = _chatBotContext.TheProjects.Find(projectId);
            return prj.Id;
        }

        public int GetResponseOwnerId(int responseId)
        {
            var resp = _chatBotContext.BotResponses.Find(responseId);
            var ctx = _chatBotContext.Contexts.Find(resp.PatternId);
            var prj = _chatBotContext.TheProjects.Find(ctx.ProjectId);
            return prj.OwnerId;
        }
    }
}
