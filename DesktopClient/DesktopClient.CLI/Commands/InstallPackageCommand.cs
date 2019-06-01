using DesktopClient.DAL;
using Microsoft.Extensions.Logging;
using PackagesService.API.Client;
using PackagesService.Domain;
using System.IO.Abstractions;

namespace DesktopClient.Domain.Commands
{
    class InstallPackageCommand
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly APIClient _apiClient;

        public InstallPackageCommand(ILogger logger, IFileSystem fileSystem, APIClient apiClient)
        {
            _logger = logger;
            _fileSystem = fileSystem;
            _apiClient = apiClient;
        }

        public async void ExecuteAsync(string targetPackageName)
        {
            VerifyPackageIsValid(targetPackageName);

            // TODO Should this IPackageStore be DI'd?
            IPackageStore packageStore = new LocalPackageStore(new PackageStoreData(_logger, _fileSystem, _fileSystem.DirectoryInfo.FromDirectoryName("/")));

            _logger.LogInformation(">>>>> Installing package <{TargetPackage}>... <<<<<", targetPackageName);
            var package = await _apiClient.GetLatestPackageZip(targetPackageName);

            packageStore.AddPackage(package);
        }

        private void VerifyPackageIsValid(string targetPackageName)
        {
            _logger.LogInformation("Verifying that the target package exists...");

            var searchSucceeded = _apiClient.TryFindPackageByName(targetPackageName, out Package package);

            if (searchSucceeded)
            {
                if (package == default(Package))
                {
                    _logger.LogError($"Could not find package {targetPackageName}");
                }
            }
            else
            {
                _logger.LogError("Search did not succeed. Is the API available? Is your Internet connection working?");
            }
        }
    }
}
