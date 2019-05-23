using PackagesService.Domain;
using System;

namespace DesktopClient.Domain
{
    /// <summary>
    /// Represents a store of <see cref="Package"/>s on the local filesystem.
    /// </summary>
    public class LocalPackageStore : IPackageStore
    {
        private readonly IPackageStoreData _packageStoreData;

        public LocalPackageStore(IPackageStoreData packageStoreData)
        {
            _packageStoreData = packageStoreData ?? throw new ArgumentNullException(nameof(packageStoreData));
        }

        public void Initialize()
        {
            _packageStoreData.AddRootFolder();
        }

        public void AddPackage(PackageRevision packageRev)
        {
            if (packageRev == null)
            {
                throw new ArgumentNullException(nameof(packageRev));
            }

            _packageStoreData.AddPackageRootFolder(packageRev);
            _packageStoreData.AddPackageRevisionFolder(packageRev);
            _packageStoreData.ExtractPackageRevisionToFolder(packageRev);
        }        
    }
}