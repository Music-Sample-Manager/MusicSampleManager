using Microsoft.Extensions.Logging;

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

        public void Execute(string targetPackage)
        {
            _logger.LogInformation(">>>>> Installing package <{TargetPackage}>... <<<<<", targetPackage);

            VerifyPackageIsValid(targetPackage);
        }

        private void VerifyPackageIsValid(string targetPackage)
        {
            _logger.LogInformation("Verifying that the target package exists...");

            var result = _apiClient.FindPackageByName(targetPackage);
        }
    }
}