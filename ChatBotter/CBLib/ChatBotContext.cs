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
        public ChatBotContext() : base("chatBotConn")
        {

        }

        public DbSet<TheProject> TheProjects { get; set; }
        public DbSet<BotResponse> BotResponses { get; set; }
        public DbSet<Context> Contexts { get; set; }

        #region mappings

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TheProject>().ToTable("TheProjects");
            modelBuilder.Entity<TheProject>().HasKey(t => t.Id);


            modelBuilder.Entity<BotResponse>().ToTable("BotResponses");
            modelBuilder.Entity<BotResponse>().HasKey(t => t.Id);


            modelBuilder.Entity<Context>().ToTable("Contexts");
            modelBuilder.Entity<Context>().HasKey(t => t.Id);
            modelBuilder.Entity<Context>().Ignore(t => t.ExpressionsList);
            modelBuilder.Entity<Context>().Ignore(t => t.InversedPolishQueue);
            modelBuilder.Entity<Context>().Ignore(t => t.CurrentStringToMatchWithTree);
            modelBuilder.Entity<Context>().Ignore(t => t.Root);
            modelBuilder.Entity<Context>().Ignore(t => t.Root);
            modelBuilder.Entity<Context>().Ignore(t => t.Root);
            modelBuilder.Entity<Context>().Ignore(t => t.Root);
            modelBuilder.Entity<Context>().Ignore(t => t.Root);
        }

        #endregion
    }
}
