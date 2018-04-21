using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.Helpers
{
    public interface IAccessVerifier
    {
        bool HasAccess(int userId);
    }
}
