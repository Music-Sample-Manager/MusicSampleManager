using PackagesService.Domain;
using System.Collections.Generic;

namespace DesktopClient.Domain
{
    public interface IPackageStore
    {
        List<PackageStoreEntry> Entries { get; }

        void AddPackageRevision(PackageRevision packageRev);
    }
}