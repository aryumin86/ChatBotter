using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// Wrapper around the context.
    /// </summary>
    [Table("Contexts")]
    public class ContextWrapper
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ProjectId { get; set; }

        public int Priority { get; set; }

        public string ExpressionStr { get; set; }

        [NotMapped]
        public Context Ctx { get; private set; }

        /// <summary>
        /// It is used to create real Context object using ExpressionStr.
        /// </summary>
        /// <param name="parser"></param>
        public void InitExistingContext(AbstractPatternParser parser)
        {
            this.Ctx = new Context(this.ExpressionStr, parser);
        }

        /// <summary>
        /// It is used to create new Context (e.g. before saving ContextWrapper object to database).
        /// </summary>
        public void InitNewContext()
        {
            //TODO here should be used main Context constructor and ExpressionStr prop set.
        }
    }
}
