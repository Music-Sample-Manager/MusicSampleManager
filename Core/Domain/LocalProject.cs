using System;
using System.IO.Abstractions;

namespace Domain
{
    public class LocalProject
    {
        private readonly IFileSystem FileSystem;
        public readonly DirectoryInfoBase RootFolder;

        public LocalProject(IFileSystem fileSystem, string rootFolder)
        {
            FileSystem = fileSystem;

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (rootFolder == null)
            {
                throw new ArgumentNullException(nameof(rootFolder));
            }

            if (!FileSystem.Directory.Exists(rootFolder))
            {
                throw new ArgumentException($"{rootFolder} does not exist. Invalid project root folder.");
            }

            RootFolder = FileSystem.DirectoryInfo.FromDirectoryName(rootFolder);
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

            return $"{RootFolder}\\{LocalPackageStore.RootFolderName}\\{packageRevision.Package.Identifier}\\{packageRevision.VersionNumber}";
        }
    }
}