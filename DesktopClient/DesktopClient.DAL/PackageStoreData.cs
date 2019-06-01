using System;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using DesktopClient.Domain;
using Microsoft.Extensions.Logging;
using PackagesService.Domain;

namespace DesktopClient.DAL
{
    public class PackageStoreData : IPackageStoreData
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        private readonly IDirectoryInfo _rootDirectory;
        IDirectoryInfo IPackageStoreData.RootDirectory
        {
            get => _rootDirectory;
        }

        public const string RootFolderName = "samplePackages";

        public PackageStoreData(ILogger logger, IFileSystem fileSystem, IDirectoryInfo rootDirectory)
        {
            if (rootDirectory == null)
            {
                throw new ArgumentNullException(nameof(rootDirectory));
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (!fileSystem.Directory.Exists(rootDirectory.FullName))
            {
                throw new ArgumentException(nameof(rootDirectory));
            }

            _logger = logger;
            _fileSystem = fileSystem;
            _rootDirectory = rootDirectory;
        }

        public bool RootFolderExists()
        {
            var allSubFolders = ((IPackageStoreData)this).RootDirectory.EnumerateDirectories().ToList();
            var packagesFolder = allSubFolders.SingleOrDefault(sf => sf.Name == RootFolderName);

            return packagesFolder != null;
        }

        public string PackageRootFolder(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (package.Identifier == null)
            {
                throw new ArgumentNullException(nameof(package.Identifier));
            }

            return $"{((IPackageStoreData)this).RootDirectory.FullName}\\{RootFolderName}\\{package.Identifier}";
        }

        public void AddPackageFolder(Package package)
        {
            ((IPackageStoreData)this).RootDirectory.CreateSubdirectory(PackageRootFolder(package));
        }

        public void AddPackageRevisionFolder(PackageRevision packageRev)
        {
            ((IPackageStoreData)this).RootDirectory.CreateSubdirectory(PackageRevisionFolder(packageRev));
        }

        public void ExtractPackageRevisionToFolder(PackageRevision packageRev)
        {
            packageRev.Contents.ExtractToDirectory(PackageRevisionFolder(packageRev));
        }

        public string PackageRevisionFolder(PackageRevision packageRevision)
        {
            if (packageRevision == null)
            {
                throw new ArgumentNullException(nameof(packageRevision));
            }

            return $"{((IPackageStoreData)this).RootDirectory.FullName}\\{packageRevision.Package.Identifier}\\{packageRevision.VersionNumber}";
        }
    }
}