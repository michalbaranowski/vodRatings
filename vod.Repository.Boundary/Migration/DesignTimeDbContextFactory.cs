﻿using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace vod.Repository.Boundary.Migration
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connstr = configuration.GetConnectionString("VodConnection");
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connstr);
            return new AppDbContext(builder.Options);
        }
    }
}