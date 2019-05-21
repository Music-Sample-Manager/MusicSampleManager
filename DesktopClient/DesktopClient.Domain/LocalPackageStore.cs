using PackagesService.Domain;
using System;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

namespace DesktopClient.Domain
{
    /// <summary>
    /// Represents a store of <see cref="Package"/>s on the local filesystem.
    /// </summary>
    public class LocalPackageStore
    {
        public const string RootFolderName = "samplePackages";

        private readonly IFileSystem _fileSystem;
        private readonly LocalProject PackageProject;

        public LocalPackageStore(IFileSystem fileSystem, LocalProject packageStoreProject)
        {
            _fileSystem = fileSystem;
            PackageProject = packageStoreProject ?? throw new ArgumentNullException(nameof(packageStoreProject));
        }

        //TODO // Most of the methods in this class are details. I need to set up some boundaries.
             // First, I need a high-level, abstract, logical domain to work with - an IPackageStore.
             // The IPackageStore can AddPackage()s, RemovePackage()s, etc. The FileSystemPackageStore
             // those tasks in the filesystem.

        public bool RootFolderExists()
        {
            var allSubFolders = PackageProject.RootFolder.EnumerateDirectories().ToList();
            var packagesFolder = allSubFolders.SingleOrDefault(sf => sf.Name == RootFolderName);

            return packagesFolder != null;
        }

        public void CreateRootFolder()
        {
            if (RootFolderExists())
            {
                throw new InvalidOperationException("Can't create packages folder when it already exists.");
            }
            else
            {
                PackageProject.RootFolder.CreateSubdirectory(RootFolderName);
            }
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

            return $"{PackageProject.RootFolder.FullName}\\{RootFolderName}\\{package.Identifier}";
        }

        public void AddPackage(PackageRevision packageRev)
        {
            if (packageRev == null)
            {
                throw new ArgumentNullException(nameof(packageRev));
            }

            _fileSystem.Directory.CreateDirectory(PackageRootFolder(packageRev.Package));
            _fileSystem.Directory.CreateDirectory(PackageProject.PackageRevisionFolder(packageRev));

            packageRev.Contents.ExtractToDirectory(PackageProject.PackageRevisionFolder(packageRev));
        }
    }
}