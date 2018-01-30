using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// Response of a bot to a user.
    /// </summary>
    [Table("BotResponses")]
    public class BotResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// Importance of the response in comparison with other
        /// responses dealt with that pattern.
        /// </summary>
        public int Priority { get; set; }

        public string ResponseText { get; set; }

        public int TheProjectId { get; set; }

        public Context Pattern { get; set; }

        public int PatternId { get; set; }

        public BotResponse(int theProjectId, string responseText, int priority = 1)
        {
            TheProjectId = theProjectId;
            Priority = priority;
            ResponseText = responseText;
        }
    }
}
