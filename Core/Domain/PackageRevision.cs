using System.IO.Compression;

namespace Domain
{
    public class PackageRevision
    {
        public string VersionNumber { get; private set; }

        public ZipArchive Contents { get; private set; }
    }
}