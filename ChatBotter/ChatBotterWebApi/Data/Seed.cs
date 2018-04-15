using System;
using System.Collections.Generic;
using CBLib;
using CBLib.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ChatBotterWebApi.Data
{
    public class Seed
    {
        private readonly ChatBotContext _context;
        private IConfiguration _config { get; }

        public Seed(ChatBotContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public void SeedUSers()
        {
            if (!bool.Parse(_config.GetSection("AppSettings:ActivateUsersSeeding").Value) == true)
                return;

            //_context.Users.RemoveRange(_context.Users);
            //_context.SaveChanges();

            var usersData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(usersData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UserName = user.UserName.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
