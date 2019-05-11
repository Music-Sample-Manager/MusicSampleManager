using System.Collections.Generic;
using System.IO.Compression;

namespace PackagesService.Domain
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAll();

        Package FindByName(string packageName);

        // TODO This should be pushed out to a controller action.
        ZipArchive DownloadLatestByName(string packageName);
    }
}