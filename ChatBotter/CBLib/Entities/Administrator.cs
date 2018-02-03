using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// ARM admin.
    /// </summary>
    public class Administrator
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
