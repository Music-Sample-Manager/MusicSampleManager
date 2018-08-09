using Domain;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace Clients.Common.Commands
{
    public class InstallPackageCommand
    {
        private readonly ILogger _logger;
        private readonly APIClient.APIClient _apiClient;

        public InstallPackageCommand(ILogger logger, APIClient.APIClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public void Execute(string targetPackageName)
        {
            VerifyPackageIsValid(targetPackageName);

            _logger.LogInformation("Checking if `packages` folder exists....");
            var fileSystem = new FileSystem();
            var localPackageStore = new LocalPackageStore(fileSystem, new LocalProject(fileSystem, fileSystem.Directory.GetCurrentDirectory()));
            var packagesFolderExists = localPackageStore.RootFolderExists();

            if (packagesFolderExists)
            {
                _logger.LogInformation("Packages folder found. Using existing folder.");
            }
            else
            {
                _logger.LogInformation("Packages folder not found. Creating new packages folder.");
                localPackageStore.CreateRootFolder();
            }

            _logger.LogInformation(">>>>> Installing package <{TargetPackage}>... <<<<<", targetPackageName);
            var package = _apiClient.DownloadPackage(targetPackageName, string.Empty);

            localPackageStore.AddPackage(package);
        }

        private void VerifyPackageIsValid(string targetPackageName)
        {
            _logger.LogInformation("Verifying that the target package exists...");

            var result = _apiClient.FindPackageByName(targetPackageName);
        }
    }
}