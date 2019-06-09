using System;
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
            Package = package ?? throw new ArgumentNullException(nameof(package));
            VersionNumber = versionNumber ?? throw new ArgumentNullException(nameof(versionNumber));
            Contents = contents ?? throw new ArgumentNullException(nameof(contents));
        }

        public Uri ContentsUri()
        {
            return new Uri($"https://msmpackagedbsa.blob.core.windows.net/all-packages/" + Package.Identifier + "/" + VersionNumber.ToString() + ".sf2");
        }
    }
}