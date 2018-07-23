using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageDatabase
{
    public class DbPackageRepository : IPackageRepository
    {
        public List<Package> GetAll()
        {
            throw new NotImplementedException();
        }

        public Package FindByName(string packageName)
        {
            var allPackages = GetAll();

            var result = allPackages.Where(p => p.Identifier == packageName);

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
    }
}