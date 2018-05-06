using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Data
{
    public interface IValidationHelperRepository
    {
        int GetContextOwnerId(int contextId);
        int GetResponseOwnerId(int responseId);
        int GetGreetingOwnerId(int greetingId);
        int GetProjectOwnerId(int projectId);
        int GetFarewellOwnerId(int farewellId);
    }
}
