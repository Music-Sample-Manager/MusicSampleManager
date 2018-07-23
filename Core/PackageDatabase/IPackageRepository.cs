using Domain;
using System.Collections.Generic;

namespace PackageDatabase
{
    public interface IPackageRepository
    {
        List<Package> GetAll();

        Package FindByName(string packageName);
    }
}
