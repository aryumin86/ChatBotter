using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ProjectTitle")]
        public string ProjectTitle { get; set; }

        [Column("ProjectDescription")]
        public string ProjectDescription { get; set; }

        [Column("OwnerId")]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public TheUser TheUser { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
