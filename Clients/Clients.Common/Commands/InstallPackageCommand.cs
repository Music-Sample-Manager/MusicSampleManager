using Domain;
using Microsoft.Extensions.Logging;
using System.IO;
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
            var localPackageStore = new LocalPackageStore(new FileSystem(), Directory.GetCurrentDirectory());
            var packagesFolderExists = localPackageStore.PackagesFolderExists();

            if (packagesFolderExists)
            {
                _logger.LogInformation("Packages folder found. Using existing folder.");
            }
            else
            {
                _logger.LogInformation("Packages folder not found. Creating new packages folder.");
                localPackageStore.CreatePackagesFolder();
            }

            _logger.LogInformation(">>>>> Installing package <{TargetPackage}>... <<<<<", targetPackageName);
            _apiClient.DownloadPackage(targetPackageName, string.Empty);
        }

        private void VerifyPackageIsValid(string targetPackageName)
        {
            _logger.LogInformation("Verifying that the target package exists...");

            var result = _apiClient.FindPackageByName(targetPackageName);
        }
    }
}