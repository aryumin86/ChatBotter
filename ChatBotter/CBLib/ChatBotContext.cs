using CBLib.Entities;
using Microsoft.EntityFrameworkCore;
using SamplesToTextsMatcher;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBLib
{
    public class ChatBotContext : DbContext
    {
        public ChatBotContext(DbContextOptions<ChatBotContext> options) : base(options)
        {

        }

        public DbSet<TheProject> TheProjects { get; set; }
        public DbSet<BotResponse> BotResponses { get; set; }
        public DbSet<Context> Contexts { get; set; }

        #region mappings

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        #endregion
    }
}
