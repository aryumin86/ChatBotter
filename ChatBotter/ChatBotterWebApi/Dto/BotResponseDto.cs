using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.DTO
{
    public class BotResponseDto
    {
        public int Id { get; set; }

        public int Priority { get; set; }

        public string ResponseText { get; set; }

        public int TheProjectId { get; set; }

        public int PatternId { get; set; }
    }
}
