using System;
using System.IO;
using CBLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChatBotterWebApi
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChatBotContext>
    {
        public ChatBotContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ChatBotContext>();
            var connectionString = configuration.GetConnectionString("chatbotter_mysql_conn");
            //builder.UseSqlServer(connectionString);
            builder.UseMySql(connectionString);
            return new ChatBotContext(builder.Options);
        }
    }
}
