using System;
using System.Collections.Generic;
using System.Text;
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

        public Context Context { get; set; }
    }
}
