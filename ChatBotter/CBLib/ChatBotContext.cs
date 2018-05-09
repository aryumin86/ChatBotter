using CBLib.Entities;
using Microsoft.EntityFrameworkCore;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CBLib
{
    public class ChatBotContext : DbContext
    {
        public ChatBotContext(DbContextOptions options) : base(options) {

        }

        public DbSet<TheProject> TheProjects { get; set; }
        public DbSet<BotResponse> BotResponses { get; set; }
        public DbSet<ContextWrapper> Contexts { get; set; }
        public DbSet<Greeting> Greetings { get; set; }
        public DbSet<Farewell> Farewells { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DefaultBotResponse> DefaultBotResponses { get; set; }
    }
}
