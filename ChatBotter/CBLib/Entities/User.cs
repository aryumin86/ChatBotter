using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// User of the service. Mere user or admin.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// This user can do anything with all projects of app on server.
        /// </summary>
        /// <value><c>true</c> if app admin; otherwise, <c>false</c>.</value>
        public bool AppAdmin { get; set; } = false;
    }
}
