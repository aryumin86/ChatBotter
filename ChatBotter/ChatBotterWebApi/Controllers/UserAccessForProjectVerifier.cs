using System;
using CBLib;
using System.Linq;

namespace ChatBotterWebApi.Controllers
{
    public class UserAccessForProjectVerifier
    {
        public bool Verify(int userId, int projectId, ChatBotContext context){
            var prj = context.TheProjects.FirstOrDefault(p => p.Id == projectId);
            if (prj == null)
                return false;
            return prj.OwnerId == userId;
        }
    }
}
