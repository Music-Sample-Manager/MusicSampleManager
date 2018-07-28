using Domain;
using System.Collections.Generic;
using System.IO.Compression;

namespace PackageDatabase
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAll();

        Package FindByName(string packageName);

        ZipArchive DownloadLatestByName(string packageName);
    }
}
