using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBotterWebApi.DTO
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
