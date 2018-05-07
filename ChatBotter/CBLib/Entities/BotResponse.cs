using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CBLib.Entities
{
    /// <summary>
    /// Response of a bot to a user.
    /// </summary>
    [Table("BotResponses")]
    public class BotResponse
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Importance of the response in comparison with other
        /// responses dealt with that pattern.
        /// </summary>
        [Column("Priority")]
        public int Priority { get; set; }

        [Column("ResponseText")]
        public string ResponseText { get; set; }

        [Column("TheProjectId")]
        public int TheProjectId { get; set; }

        [ForeignKey("TheProjectId")]
        public TheProject TheProject { get; set; }

        [ForeignKey("PatternId")]
        public ContextWrapper Pattern { get; set; }

        [Column("PatternId")]
        public int PatternId { get; set; }
    }
}
