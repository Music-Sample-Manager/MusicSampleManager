using System.IO.Abstractions;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// Represents a store of <see cref="Package"/>s on the local filesystem.
    /// </summary>
    public class LocalPackageStore
    {
        private const string packagesFolderName = "samplePackages";

        private readonly IFileSystem _fileSystem;
        private readonly DirectoryInfoBase _projectRootDirectory;

        public LocalPackageStore(IFileSystem fileSystem, string projectRootDirectory)
        {
            _fileSystem = fileSystem;
            _projectRootDirectory = _fileSystem.DirectoryInfo.FromDirectoryName(projectRootDirectory);
        }

        public bool PackagesFolderExists()
        {
            var allSubFolders = _projectRootDirectory.EnumerateDirectories().ToList();
            var packagesFolder = allSubFolders.SingleOrDefault(sf => sf.Name == packagesFolderName);

            return packagesFolder != null;
        }
    }
}