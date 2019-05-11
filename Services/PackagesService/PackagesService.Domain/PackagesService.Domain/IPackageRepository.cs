using System.Collections.Generic;

namespace PackagesService.Domain
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAll();

        Package FindByName(string packageName);

        PackageRevision FindLatestRevisionByPackageName(string packageName);
    }
}