using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotterWebApi.DTO
{
    public class TheProjectDto
    {
        public int Id { get; set; }

        [Required]
        public string ProjectTitle { get; set; }

        [Required]
        public string ProjectDescription { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
