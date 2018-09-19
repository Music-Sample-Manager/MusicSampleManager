using System.IO.Compression;
using SemVer;

namespace Domain
{
    public class PackageRevision
    {
        public Package Package { get; }

        public Version VersionNumber { get; }

        public ZipArchive Contents { get; }

        public PackageRevision(Package package, Version versionNumber, ZipArchive contents)
        {
            Package = package;
            VersionNumber = versionNumber;
            Contents = contents;
        }
    }
}