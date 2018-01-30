using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib.Entities
{
    /// <summary>
    /// User of service (and may be ARM too).
    /// </summary>
    public class TheUser
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
