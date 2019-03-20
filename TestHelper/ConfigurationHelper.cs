using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestHelper
{
    public class ConfigurationHelper
    {
        public const string USER_SECRETS_GUID = "9dcc88f8-22e2-47e1-80a0-852e01bcc389";

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                    .SetBasePath(outputPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets(USER_SECRETS_GUID)
                    .AddEnvironmentVariables()
                    .Build();
        }

        public static AssetRegisterApiConfiguration GetAssetRegisterApiConfiguration(string outputPath)
        {
            var configuration = new AssetRegisterApiConfiguration();
            var iConfig = GetIConfigurationRoot(outputPath);
            iConfig.Bind(configuration);
            return configuration;
        }
    }
}
