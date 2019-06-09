using PackagesService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackagesService.UseCases
{
    public class ListAllPackagesForAuthor
    {
        public List<Package> Execute(IPackageRepository packageRepository, int authorId)
        {
            if (packageRepository == null)
            {
                throw new ArgumentNullException(nameof(packageRepository));
            }

            if (authorId <= 0)
            {
                throw new ArgumentException($"Must provide a valid authorId: {authorId}");
            }

            var allPackages = packageRepository.GetAll();
            var packagesForAuthor = allPackages.Where(p => p.AuthorId == authorId);
            return packagesForAuthor.ToList();
        }
    }
}
