﻿using Clients.Common.Commands;
using Microsoft.Extensions.Logging;
using System;

namespace MusicSampleManager.CLI
{
    class Program
    {
        private static ILogger _logger;
        private static APIClient.APIClient _apiClient;

        static void Main(string[] args)
        {
            // TODO UNDO THIS LATER:
            args = new[] { "Install", "VirtualPlayingOrchestra" };

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
            _apiClient = new APIClient.APIClient();
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