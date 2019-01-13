using System;
using System.Net.Http;
using Clients.Common.Commands;
using Microsoft.Extensions.Logging;

namespace MusicSampleManager.CLI
{
    class Program
    {
        private static ILogger _logger;
        private static APIClient.APIClient _apiClient;

        static void Main(string[] args)
        {
            ConfigureDI();

            _logger.LogInformation("msm {Command}", string.Join(" ", args));
            ParseArguments(args);

            Console.ReadLine();
        }

        private static void ConfigureDI()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            _logger = loggerFactory.CreateLogger("InformationalLogs");

            // TODO This should be easily configurable for use in various environments, etc.
            _apiClient = new APIClient.APIClient("https://localhost:44349", new HttpClient());
        }

        private static void ParseArguments(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                DisplayHelpMessage();
            }

            if (args.Length == 2)
            {
                if (args[0] == "Install")
                {
                    var command = new InstallPackageCommand(_logger, _apiClient);
                    command.Execute(args[1]);
                }
            }

            if (args.Length > 2)
            {
                _logger.LogError("Too many arguments provided.");
                DisplayHelpMessage();
            }
        }

        private static void DisplayHelpMessage()
        {
            _logger.LogInformation("Usage: msm <command>");
            _logger.LogInformation(Environment.NewLine);
            _logger.LogInformation("<command>s:");
            _logger.LogInformation("Install");
            _logger.LogInformation("Uninstall");
        }
    }
}