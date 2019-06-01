using PackagesService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesktopClient.Domain
{
    /// <summary>
    /// Represents a store of <see cref="Package"/>s on the local filesystem.
    /// </summary>
    public class LocalPackageStore : IPackageStore
    {
        private readonly IPackageStoreData _packageStoreData;

        public List<PackageStoreEntry> Entries { get; private set; } = new List<PackageStoreEntry>();

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

            if (Entries.ToList().Exists(e => e.Package.Identifier == packageRev.Package.Identifier))
            {
               Entries.Single(e => e.Package.Identifier == packageRev.Package.Identifier).PackageRevisions.Add(packageRev);
            }
            else
            {
                Entries.Add(new PackageStoreEntry(packageRev.Package, new List<PackageRevision> { packageRev }));
            }
        }
    }
}