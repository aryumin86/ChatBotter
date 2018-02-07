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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Context>().ToTable("Contexts");
            //modelBuilder.Entity<Context>().Ignore(b => b.CurrentStringToMatchWithTree);
            //modelBuilder.Entity<Context>().Ignore(b => b.ExpressionsList);
            //modelBuilder.Entity<Context>().Ignore(b => b.InversedPolishQueue);
            //modelBuilder.Entity<Context>().Ignore(b => b.Root);

            //modelBuilder.Entity<Context>().Property(b => b.Id).HasColumnName("Id");
            //modelBuilder.Entity<Context>().Property(b => b.Title).HasColumnName("Title");
            //modelBuilder.Entity<Context>().Property(b => b.Priority).HasColumnName("Priority");
            //modelBuilder.Entity<Context>().Property(b => b.ProjectId).HasColumnName("ProjectId");
            //modelBuilder.Entity<Context>().Property(b => b.Root.ResStringExpression).HasColumnName("ResStringExpression");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("mysql_conn");
        }
    }
}
