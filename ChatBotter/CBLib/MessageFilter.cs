using System;
using System.Collections.Generic;
using System.Text;
using CBLib.Entities;
using SamplesToTextsMatcher;

namespace CBLib
{
    /// <summary>
    /// Filters for greetings, rudeness, insults etc.
    /// This filter should be used at first before any context matching.
    /// </summary>
    public class MessageFilter
    {
        public int Id { get; set; } 

        public string Title { get; set; }

        /// <summary>
        /// Tuple: projectId, context for matching the user message, response of bot if matched. 
        /// </summary>
        List<Tuple<int, Context, BotResponse>> filterBehavior = new List<Tuple<int, Context, BotResponse>>();
    }
}
