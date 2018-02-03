using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// Bot's project.
    /// </summary>
    [Table("Projects")]
    public class TheProject
    {
        public int Id { get; set; }

        public string ProjectTitle { get; set; }

        public string ProjectDescription { get; set; }

        public int OwnerId { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Greeting of the user.
        /// </summary>
        /// <value>The greeting.</value>
        public string Greeting { get; set; }

        /// <summary>
        /// Goodbye words.
        /// </summary>
        /// <value>The farewell.</value>
        public string Farewell { get; set; }
    }
}
