using System.IO.Compression;

namespace Domain
{
    public class PackageRevision
    {
        public Package Package { get; }

        public string VersionNumber { get; }

        public ZipArchive Contents { get; }

        public PackageRevision(Package package, string versionNumber, ZipArchive contents)
        {
            Package = package;
            VersionNumber = versionNumber;
            Contents = contents;
        }
    }
}