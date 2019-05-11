using System.IO.Compression;
using Semver;

namespace PackagesService.Domain
{
    public class PackageRevision
    {
        public Package Package { get; }

        public SemVersion VersionNumber { get; }

        public ZipArchive Contents { get; }

        public PackageRevision(Package package, SemVersion versionNumber, ZipArchive contents)
        {
            Package = package;
            VersionNumber = versionNumber;
            Contents = contents;
        }
    }
}