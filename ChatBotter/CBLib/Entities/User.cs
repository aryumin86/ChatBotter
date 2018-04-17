using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// User of the service. Mere user or admin.
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// This user can do anything with all projects of app on server.
        /// </summary>
        /// <value><c>true</c> if app admin; otherwise, <c>false</c>.</value>
        [Column("AppAdmin")]
        public bool AppAdmin { get; set; } = false;
    }
}
