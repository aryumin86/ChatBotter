using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.DTO
{
    public class ContextDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string ExpressionRawStr { get; set; }

        public string ExpressionResStr { get; set; }
    }
}
