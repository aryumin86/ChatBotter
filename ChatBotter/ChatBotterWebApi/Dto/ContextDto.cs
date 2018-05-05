using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.DTO
{
    public class ContextDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ProjectId { get; set; }

        public int Priority { get; set; }

        public bool IsActive { get; set; }

        public string ExpressionRawStr { get; set; }

        public string ExpressionResStr { get; set; }
    }
}
