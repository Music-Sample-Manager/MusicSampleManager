using PackagesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace PackagesService.DAL
{
    public class MSMDbContext : DbContext
    {
        private readonly bool _isInMemoryTestingContext;

        public DbSet<PackageRec> Packages { get; set; }

        public DbSet<PackageRevisionRec> PackageRevisions { get; set; }

        public MSMDbContext(bool isInMemoryTestingContext)
        {
            _isInMemoryTestingContext = isInMemoryTestingContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify composite primary keys
            modelBuilder.Entity<PackageRevisionRec>()
                .HasKey(pr => new { pr.PackageId, pr.VersionNumber });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO This logic should be refactored up to depenendency injection, where the caller specifies which db provider they want to use.
            if (_isInMemoryTestingContext)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "MSMDB");
            }
            else
            {
                var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
                optionsBuilder.UseSqlServer(connString);
            }
        }
    }
}