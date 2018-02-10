using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBLib.Entities
{
    [Table("Greetings")]
    public class Greeting
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ProjectId")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public TheProject TheProject { get; set; }

        [Column("MainGreeting")]
        public string MainGreeting { get; set; }
    }
}
