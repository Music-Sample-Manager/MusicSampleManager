using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using PackagesService.Domain;

namespace DesktopClient.Domain.Tests
{
    internal class MockPackageStoreData : IPackageStoreData
    {
        private ILogger _mockLogger;
        private IDirectoryInfo mockProjectFolder;

        public MockPackageStoreData(ILogger mockLogger, IDirectoryInfo mockProjectFolder)
        {
            _mockLogger = mockLogger;
            this.mockProjectFolder = mockProjectFolder;
        }

        public IDirectoryInfo RootDirectory => throw new NotImplementedException();

        public void AddPackageRevisionFolder(PackageRevision packageRev)
        {
        }

        public void AddPackageFolder(Package package)
        {
        }

        public void AddRootFolder()
        {
        }

        public void ExtractPackageRevisionToFolder(PackageRevision packageRev)
        {
        }
    }
}