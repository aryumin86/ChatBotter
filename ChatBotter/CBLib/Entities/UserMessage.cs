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
        /// Message after text preprocessing and tokenizing.
        /// </summary>
        public string[] MessageTextAsTokensArr { get; set; }

        /// <summary>
        /// This is a project for which this message was sent from user.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Raw user's message preprocessing.
        /// </summary>
        /// <param name="pp"></param>
        public void PreProcessOfrawMessage(Func<string, string[]> pp)
        {
            this.MessageTextAsTokensArr = pp(this.MessageText);
        }
    }
}
