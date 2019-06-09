using PackagesService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackagesService.UseCases
{
    public class ListAllPackages
    {
        public List<Package> Execute(IPackageRepository packageRepository)
        {
            if (packageRepository == null)
            {
                throw new ArgumentNullException(nameof(packageRepository));
            }

            return packageRepository.GetAll().ToList();
        }
    }
}