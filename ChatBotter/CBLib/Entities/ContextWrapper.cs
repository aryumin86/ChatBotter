using SamplesToTextsMatcher;
using SamplesToTextsMatcher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("ProjectId")]
        public int ProjectId { get; set; }

        [Column("Priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Manually formed pattern.
        /// </summary>
        [Column("ExpressionRawStr")]
        public string ExpressionRawStr { get; set; }

        /// <summary>
        /// Pattern formed using raw pattern interpretation
        /// (with terms forms, all brackets for binary operations).
        /// This pattern could be used to init existing Context.
        /// </summary>
        [Column("ExpressionResStr")]
        public string ExpressionResStr { get; set; }

        [NotMapped]
        public Context Ctx { get; private set; }

        /// <summary>
        /// It is used to create real Context object using ExpressionStr.
        /// </summary>
        /// <param name="parser"></param>
        public void InitExistingContext(AbstractPatternParser parser)
        {
            this.Ctx = new Context(this.ExpressionResStr, parser);
        }

        /// <summary>
        /// It is used to create new Context (e.g. before saving ContextWrapper object to database).
        /// </summary>
        public void InitNewContext(string pattern, AbstractPatternParser parser, AbstractMorfDictionary dict, 
            bool shouldWorkWithTermsForms = false, int tokenFormsMaxNumberForAsterix = 30, Dictionary<string, LinkedList<Expression>> extras = null)
        {
            this.ExpressionRawStr = pattern;
            this.Ctx = new Context(pattern, parser, dict, shouldWorkWithTermsForms, tokenFormsMaxNumberForAsterix, extras);
        }
    }
}
