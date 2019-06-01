using PackagesService.Domain;
using System.IO.Abstractions;

namespace DesktopClient.Domain
{
    public interface IPackageStoreData
    {
        IDirectoryInfo RootDirectory { get; }

        void AddPackageFolder(Package package);
        void AddPackageRevisionFolder(PackageRevision packageRev);
        void ExtractPackageRevisionToFolder(PackageRevision packageRev);
    }
}
