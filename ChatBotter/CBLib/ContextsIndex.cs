using CBLib.Entities;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib
{
    /// <summary>
    /// Inverted index of contexts
    /// </summary>
    public class ContextsIndex
    {
        /// <summary>
        /// Dict of context. Key is context id.
        /// </summary>
        private Dictionary<int, Context> _contexts;

        /// <summary>
        /// Inverted index. Key is a token, 
        /// </summary>
        private Dictionary<string, List<int>> _index;

        /// <summary>
        /// Main context. Creates the index of containing in contexts collection
        /// terms
        /// </summary>
        /// <param name="contexts"></param>
        public ContextsIndex(IEnumerable<ContextWrapper> contexts)
        {

        }

        public bool AddContext(ContextWrapper context)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContext(ContextWrapper context)
        {
            throw new NotImplementedException();
        }

        public bool UpdateContext(ContextWrapper context)
        {
            throw new NotImplementedException();
        }

        public List<Context> GetContextsForTerms(string[] tokens, int prjId)
        {
            throw new NotImplementedException();
        }
    }
}
