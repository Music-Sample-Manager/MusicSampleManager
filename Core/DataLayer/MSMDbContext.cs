using Core.DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer
{
    public class MSMDbContext : DbContext
    {
        public DbSet<PackageRec> Packages { get; set; }

        public DbSet<PackageRevisionRec> PackageRevisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify composite primary keys
            modelBuilder.Entity<PackageRevisionRec>()
                .HasKey(pr => new { pr.PackageId, pr.VersionNumber });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            optionsBuilder.UseSqlServer(connString);
        }
    }
}