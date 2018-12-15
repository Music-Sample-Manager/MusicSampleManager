using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;
using PackageDatabase;

namespace Core.DataLayer
{
    public class DbPackageRepository : DbContext, IPackageRepository
    {
        public DbSet<PackageRec> Packages { get; set; }

        public IEnumerable<Package> GetAll() => Packages.Select(p => p.ToPackage);

        public Package FindByName(string packageName)
        {
            var allPackages = GetAll();

            var result = allPackages.Where(p => p.Identifier == packageName).ToList();

            if (result.Count() == 1)
            {
                return result.First();
            }
            else if (result.Count() > 1)
            {
                throw new Exception($"Multiple packages found with name {packageName}");
            }
            else
            {
                throw new Exception($"No packages found with name {packageName}");
            }
        }

        public ZipArchive DownloadLatestByName(string packageName)
        {
            var package = FindByName(packageName);

            return package.LatestVersion().Contents;
        }

        public DbPackageRepository(DbContextOptions<DbPackageRepository> options) : base(options) { }
    }
}