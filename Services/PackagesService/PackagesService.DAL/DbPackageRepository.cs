using System;
using System.Collections.Generic;
using System.Linq;
using PackagesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using PackagesService.Domain;

namespace PackagesService.DAL
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

        public PackageRevision FindLatestRevisionByPackageName(string packageName)
        {
            var package = FindByName(packageName);

            return package.AllVersions.Last();
        }

        public void Add(Package package)
        {
            var packageRecord = new PackageRec()
            {
                AuthorId = package.AuthorId,
                Id = package.Id,
                Identifier = package.Identifier,
                Description = package.Description
            };

            Packages.Add(packageRecord);
            SaveChanges();
        }

        public DbPackageRepository(DbContextOptions<DbPackageRepository> options) : base(options) { }
    }
}