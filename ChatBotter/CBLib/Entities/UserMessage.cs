using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib.Entities
{
    public class UserMessage
    {
        /// <summary>
        /// Text of the message.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// This is a project for which this message was sent from user.
        /// </summary>
        public int ProjectId { get; set; }
    }
}
