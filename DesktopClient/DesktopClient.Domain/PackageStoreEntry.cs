using PackagesService.Domain;
using System.Collections.Generic;

namespace DesktopClient.Domain
{
    public class PackageStoreEntry
    {
        public readonly Package Package;
        public readonly List<PackageRevision> PackageRevisions;

        public PackageStoreEntry(Package package, List<PackageRevision> packageRevisions)
        {
            Package = package;
            PackageRevisions = packageRevisions;
        }
    }
}