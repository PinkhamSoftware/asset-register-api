using System.Data;
using DependencyInjection;
using HomesEngland.BackgroundProcessing;
using HomesEngland.Domain;
using HomesEngland.Domain.Factory;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Assets.Developer;
using HomesEngland.Gateway.Assets.Region;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.JWT;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Notifications;
using HomesEngland.Gateway.Notify;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.AuthenticateUser;
using HomesEngland.UseCase.AuthenticateUser.Impl;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Impl;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Impl;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GenerateAssets.Impl;
using HomesEngland.UseCase.GenerateAssets.Models;
using HomesEngland.UseCase.GetAccessToken;
using HomesEngland.UseCase.GetAccessToken.Impl;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Impl;
using HomesEngland.UseCase.GetAssetDevelopers;
using HomesEngland.UseCase.GetAssetDevelopers.Impl;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.GetAssetRegions.Impl;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Impl;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.ImportAssets.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HomesEngland.UseCase.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Main
{
    public class AssetRegister : DependencyExporter
    {
        public const string ASSET_REGISTER_DB_CONNECTION_STRING_KEY = "ConnectionStrings:AssetRegisterApiDb";

        public AssetRegister(IConfiguration configuration)
            : base (configuration)
        {
        }

        private ServiceProvider _serviceProvider;

        /// <summary>
        /// These dependencies are local to the asset register service provider.
        /// They are exported to the WebAPI service provider via Startup.cs.
        /// Asset Register contains its own service provider for the purpose of integration testing.
        /// </summary>
        protected override void ConstructHiddenDependencies()
        {
            var serviceCollection = new ServiceCollection();

            ExportDependencies((type, provider) => serviceCollection.AddTransient(type, _ => provider()));

            ExportTypeDependencies((type, provider) => serviceCollection.AddTransient(type, provider));

            ExportSingletonDependencies((type, provider) => serviceCollection.AddTransient(type, _ => provider()));

            ExportSingletonTypeDependencies((type, provider) => serviceCollection.AddTransient(type, provider));

            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<AssetRegisterContext>(opts => opts.UseSqlServer(Configuration[ASSET_REGISTER_DB_CONNECTION_STRING_KEY]));
                        
            serviceCollection.AddHostedService<BackgroundProcessor>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void RegisterAllExportedDependencies()
        {
            var connectionString = Configuration[ASSET_REGISTER_DB_CONNECTION_STRING_KEY];
            RegisterExportedDependency<IGetAssetUseCase, GetAssetUseCase>();
            RegisterExportedDependency<IAssetReader>(() => new EFAssetGateway(connectionString));
            RegisterExportedDependency<AssetRegisterContext>(() =>
            {
                var options = new DbContextOptionsBuilder<AssetRegisterContext>();
                options.UseSqlServer(connectionString);
                return new AssetRegisterContext(options.Options);
            });
            RegisterExportedDependency<ISearchAssetUseCase, SearchAssetUseCase>();
            RegisterExportedDependency<IAssetSearcher>(() => new EFAssetGateway(connectionString));
            RegisterExportedDependency<IAssetCreator>(() => new EFAssetGateway(connectionString));
            RegisterExportedDependency<IGateway<IAsset, int>>(() => new EFAssetGateway(connectionString));
            RegisterExportedDependency<ICreateAssetUseCase, CreateAssetUseCase>();
            RegisterExportedDependency<IGenerateAssetsUseCase, GenerateAssetsUseCase>();
            RegisterExportedDependency<IConsoleGenerator, ConsoleAssetGenerator>();
            RegisterExportedDependency<IInputParser<GenerateAssetsRequest>, InputParser>();
            RegisterExportedDependency<IAuthenticateUser, AuthenticateUserUseCase>();
            RegisterExportedDependency<IOneTimeAuthenticationTokenCreator>(() =>
                new EFAuthenticationTokenGateway(connectionString));
            RegisterExportedDependency<IOneTimeAuthenticationTokenReader>(() =>
                new EFAuthenticationTokenGateway(connectionString));
            RegisterExportedDependency<IOneTimeAuthenticationTokenDeleter>(() =>
                new EFAuthenticationTokenGateway(connectionString));
            RegisterExportedDependency<IOneTimeLinkNotifier, GovNotifyNotificationsGateway>();
            RegisterExportedDependency<IAssetRegisterUploadProcessedNotifier, GovNotifyNotificationsGateway>();
            RegisterExportedDependency<IAccessTokenCreator, JwtAccessTokenGateway>();
            RegisterExportedDependency<IGetAccessToken, GetAccessTokenUseCase>();

            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            RegisterExportedDependency<ILogger<ConsoleAssetGenerator>>(() =>
                new Logger<ConsoleAssetGenerator>(loggerFactory));

            RegisterExportedDependency<ILogger<IImportAssetsUseCase>>(() =>
                new Logger<IImportAssetsUseCase>(loggerFactory));

            RegisterExportedDependency<IImportAssetsUseCase, ImportAssetsUseCase>();
            RegisterExportedDependency<IConsoleImporter, ConsoleImporter>();
            RegisterExportedDependency<IFileReader<string>, TextFileReader>();
            RegisterExportedDependency<ITextSplitter, TextSplitter>();
            RegisterExportedDependency<IInputParser<ImportAssetConsoleInput>, ImportAssetInputParser>();
            RegisterExportedDependency<IFactory<CreateAssetRequest, CsvAsset>, CreateAssetRequestFactory>();
            RegisterExportedDependency<ICalculateAssetAggregatesUseCase, CalculateAssetAggregatesUseCase>();
            RegisterExportedDependency<IAssetAggregator>(() => new EFAssetGateway(connectionString));

            RegisterExportedDependency<ICreateAssetRegisterVersionUseCase, CreateAssetRegisterVersionUseCase>();
            RegisterExportedDependency<IAssetRegisterVersionCreator>(() =>
                new EFAssetRegisterVersionGateway(connectionString));
            RegisterExportedDependency<IGetAssetRegisterVersionsUseCase, GetAssetRegisterVersionsUseCase>();
            RegisterExportedDependency<IAssetRegisterVersionSearcher>(() => new EFAssetRegisterVersionGateway(connectionString));

            RegisterExportedSingletonDependency<IBackgroundProcessor, BackgroundProcessor>();

            RegisterExportedDependency<IAssetRegionLister>(() => new EFAssetGateway(connectionString));

            RegisterExportedDependency<IGetAssetRegionsUseCase, GetAssetRegionsUseCase>();

            RegisterExportedDependency<IAssetDeveloperLister>(() => new EFAssetGateway(connectionString));
            RegisterExportedDependency<IGetAssetDevelopersUseCase, GetAssetDevelopersUseCase>();
        }

        public override T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
