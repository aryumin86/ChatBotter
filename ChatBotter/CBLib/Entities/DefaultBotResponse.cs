using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CBLib.Entities
{
    [Table("DefaultBotResponses")]
    public class DefaultBotResponse
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }

        [Column("ResponseText")]
        [Required]
        public string ResponseText { get; set; }

        [Column("TheProjectId")]
        [Required]
        public int TheProjectId { get; set; }

        [ForeignKey("TheProjectId")]
        public TheProject TheProject { get; set; }
    }
}
