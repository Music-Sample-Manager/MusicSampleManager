using PackagesService.Domain;
using System.IO.Abstractions;

namespace DesktopClient.Domain
{
    public interface IPackageStoreData
    {
        IDirectoryInfo RootDirectory { get; }

        void AddRootFolder();
        void AddPackageRootFolder(PackageRevision packageRev);
        void AddPackageRevisionFolder(PackageRevision packageRev);
        void ExtractPackageRevisionToFolder(PackageRevision packageRev);
    }
}
