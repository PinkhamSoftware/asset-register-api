﻿using HomesEngland.UseCase.ImportAssets;
using Main;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HomesEngland.AssetImporter
{
    class Program
    {
        public const string USER_SECRETS_GUID = "9dcc88f8-22e2-47e1-80a0-852e01bcc389";

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets(USER_SECRETS_GUID)
                    .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var assetRegister = new AssetRegister(configuration);
            IConsoleImporter assetImporter = assetRegister.Get<IConsoleImporter>();
            assetImporter.ProcessAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
