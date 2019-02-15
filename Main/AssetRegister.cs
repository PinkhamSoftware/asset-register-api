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
using HomesEngland.Gateway.Assets.Region;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.JWT;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Notifications;
using HomesEngland.Gateway.Notify;
using HomesEngland.Gateway.Sql;
using HomesEngland.Gateway.Sql.Postgres;
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

namespace Main
{
    public class AssetRegister : DependencyExporter
    {
        private ServiceProvider _serviceProvider;

        protected override void ConstructHiddenDependencies()
        {
            var serviceCollection = new ServiceCollection();

            ExportDependencies((type, provider) => serviceCollection.AddTransient(type, _ => provider()));

            ExportTypeDependencies((type, provider) => serviceCollection.AddTransient(type, provider));

            ExportSingletonDependencies((type, provider) => serviceCollection.AddTransient(type, _ => provider()));

            ExportSingletonTypeDependencies((type, provider) => serviceCollection.AddTransient(type, provider));

            serviceCollection.AddEntityFrameworkNpgsql().AddDbContext<AssetRegisterContext>();

            
            serviceCollection.AddHostedService<BackgroundProcessor>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void RegisterAllExportedDependencies()
        {
            var databaseUrl = System.Environment.GetEnvironmentVariable("DATABASE_URL");
            RegisterExportedDependency<IDatabaseConnectionStringFormatter, PostgresDatabaseConnectionStringFormatter>();
            RegisterExportedDependency<IDatabaseConnectionFactory, PostgresDatabaseConnectionFactory>();
            RegisterExportedDependency<IDbConnection>(() =>
                new PostgresDatabaseConnectionFactory(new PostgresDatabaseConnectionStringFormatter()).Create(
                    databaseUrl));
            RegisterExportedDependency<IGetAssetUseCase, GetAssetUseCase>();
            RegisterExportedDependency<IAssetReader>(() => new EFAssetGateway(databaseUrl));
            RegisterExportedDependency<AssetRegisterContext>(() => new AssetRegisterContext(databaseUrl));
            RegisterExportedDependency<ISearchAssetUseCase, SearchAssetUseCase>();
            RegisterExportedDependency<IAssetSearcher>(() => new EFAssetGateway(databaseUrl));
            RegisterExportedDependency<IAssetCreator>(() => new EFAssetGateway(databaseUrl));
            RegisterExportedDependency<IGateway<IAsset, int>>(() => new EFAssetGateway(databaseUrl));
            RegisterExportedDependency<ICreateAssetUseCase, CreateAssetUseCase>();
            RegisterExportedDependency<IGenerateAssetsUseCase, GenerateAssetsUseCase>();
            RegisterExportedDependency<IConsoleGenerator, ConsoleAssetGenerator>();
            RegisterExportedDependency<IInputParser<GenerateAssetsRequest>, InputParser>();
            RegisterExportedDependency<IAuthenticateUser, AuthenticateUserUseCase>();
            RegisterExportedDependency<IOneTimeAuthenticationTokenCreator>(() =>
                new EFAuthenticationTokenGateway(databaseUrl));
            RegisterExportedDependency<IOneTimeAuthenticationTokenReader>(() =>
                new EFAuthenticationTokenGateway(databaseUrl));
            RegisterExportedDependency<IOneTimeAuthenticationTokenDeleter>(() =>
                new EFAuthenticationTokenGateway(databaseUrl));
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
            RegisterExportedDependency<IAssetAggregator>(() => new EFAssetGateway(databaseUrl));

            RegisterExportedDependency<ICreateAssetRegisterVersionUseCase, CreateAssetRegisterVersionUseCase>();
            RegisterExportedDependency<IAssetRegisterVersionCreator>(() =>
                new EFAssetRegisterVersionGateway(databaseUrl));
            RegisterExportedDependency<IGetAssetRegisterVersionsUseCase, GetAssetRegisterVersionsUseCase>();
            RegisterExportedDependency<IAssetRegisterVersionSearcher>(() => new EFAssetRegisterVersionGateway(databaseUrl));

            RegisterExportedSingletonDependency<IBackgroundProcessor, BackgroundProcessor>();

            RegisterExportedDependency<IAssetRegionLister>(() => new EFAssetGateway(databaseUrl));

            RegisterExportedDependency<IGetAssetRegionsUseCase, GetAssetRegionsUseCase>();
        }

        public override T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
