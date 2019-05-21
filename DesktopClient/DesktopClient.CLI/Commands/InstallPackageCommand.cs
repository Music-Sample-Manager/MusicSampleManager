﻿using Microsoft.Extensions.Logging;
using PackagesService.API.Client;
using PackagesService.Domain;
using System.IO.Abstractions;

namespace DesktopClient.Domain.Commands
{
    class InstallPackageCommand
    {
        private readonly ILogger _logger;
        private readonly APIClient _apiClient;

        public InstallPackageCommand(ILogger logger, APIClient apiClient)
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
            var package = _apiClient.GetLatestPackageZip(targetPackageName);

            localPackageStore.AddPackage(package);
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