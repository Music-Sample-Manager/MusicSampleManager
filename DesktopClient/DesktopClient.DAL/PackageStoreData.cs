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

        public const string RootFolderName = "samplePackages";

        private readonly IDirectoryInfo _rootDirectory;
        IDirectoryInfo IPackageStoreData.RootDirectory
        {
            get => _rootDirectory;
        }

        public PackageStoreData(ILogger logger, IDirectoryInfo rootDirectory)
        {
            _logger = logger;
            _rootDirectory = rootDirectory;
        }

        // TODO Why do I need separate AddRootFolder() and CreateRootFolder() methods?
        public void AddRootFolder()
        {
            _logger.LogInformation("Checking if `packages` folder exists....");

            if (RootFolderExists())
            {
                _logger.LogInformation("Packages folder found. Using existing folder.");
            }
            else
            {
                _logger.LogInformation("Packages folder not found. Creating new packages folder.");
                CreateRootFolder();
            }
        }

        public bool RootFolderExists()
        {
            var allSubFolders = ((IPackageStoreData)this).RootDirectory.EnumerateDirectories().ToList();
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
                ((IPackageStoreData)this).RootDirectory.CreateSubdirectory(RootFolderName);
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

            return $"{((IPackageStoreData)this).RootDirectory.FullName}\\{RootFolderName}\\{package.Identifier}";
        }

        public void AddPackageRootFolder(PackageRevision packageRev)
        {
            ((IPackageStoreData)this).RootDirectory.CreateSubdirectory(PackageRootFolder(packageRev.Package));
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

            if (packageRevision.VersionNumber == null)
            {
                throw new ArgumentException("Package version number cannot be null");
            }

            return $"{((IPackageStoreData)this).RootDirectory}\\{PackageRootFolder(packageRevision.Package)}\\{packageRevision.Package.Identifier}\\{packageRevision.VersionNumber}";
        }
    }
}













//public class LocalProject
//{
//    private readonly IFileSystem FileSystem;
//    public readonly IDirectoryInfo RootFolder;

//    public LocalProject(IFileSystem fileSystem, string rootFolder)
//    {
//        FileSystem = fileSystem;

//        if (fileSystem == null)
//        {
//            throw new ArgumentNullException(nameof(fileSystem));
//        }

//        if (rootFolder == null)
//        {
//            throw new ArgumentNullException(nameof(rootFolder));
//        }

//        if (!FileSystem.Directory.Exists(rootFolder))
//        {
//            throw new ArgumentException($"{rootFolder} does not exist. Invalid project root folder.");
//        }

//        //RootFolder = FileSystem.DirectoryInfo.FromDirectoryName(rootFolder);
//    }
//}