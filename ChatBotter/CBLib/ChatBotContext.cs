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

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration config = builder.Build();
            var connString = config.GetConnectionString("chatbotter_mysql_conn");

            optionsBuilder.UseMySql(connString);

        }
        */
    }
}
