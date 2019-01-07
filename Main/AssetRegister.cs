using System.Data;
using DependencyInjection;
using HomesEngland.Domain;
using HomesEngland.Domain.Factory;
using HomesEngland.Gateway;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.Gateway.Sql.Postgres;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Impl;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GenerateAssets.Impl;
using HomesEngland.UseCase.GenerateAssets.Models;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Impl;
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

            serviceCollection.AddEntityFrameworkNpgsql().AddDbContext<AssetRegisterContext>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void RegisterAllExportedDependencies()
        {
            var databaseUrl = System.Environment.GetEnvironmentVariable("DATABASE_URL");
            RegisterExportedDependency<IDatabaseConnectionStringFormatter, PostgresDatabaseConnectionStringFormatter>();
            RegisterExportedDependency<IDatabaseConnectionFactory, PostgresDatabaseConnectionFactory>();
            RegisterExportedDependency<IDbConnection>(()=> new PostgresDatabaseConnectionFactory(new PostgresDatabaseConnectionStringFormatter()).Create(databaseUrl));
            RegisterExportedDependency<IGetAssetUseCase, GetAssetUseCase>();
            RegisterExportedDependency<IAssetReader, EFAssetGateway>();
            RegisterExportedDependency<AssetRegisterContext>(()=> new AssetRegisterContext(databaseUrl));
            RegisterExportedDependency<ISearchAssetUseCase, SearchAssetUseCase>();
            RegisterExportedDependency<IAssetSearcher, EFAssetGateway>();
            RegisterExportedDependency<IAssetCreator, EFAssetGateway>();
            RegisterExportedDependency<IGateway<IAsset, int>, EFAssetGateway>();
            RegisterExportedDependency<ICreateAssetUseCase, CreateAssetUseCase>();
            RegisterExportedDependency<IGenerateAssetsUseCase, GenerateAssetsUseCase>();
            RegisterExportedDependency<IConsoleGenerator, ConsoleAssetGenerator>();
            RegisterExportedDependency<IInputParser<GenerateAssetsRequest>, InputParser>();

            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            RegisterExportedDependency<ILogger<ConsoleAssetGenerator>>(() => new Logger<ConsoleAssetGenerator>(loggerFactory));

            RegisterExportedDependency<ILogger<IImportAssetsUseCase>>(() => new Logger<IImportAssetsUseCase>(loggerFactory));

            RegisterExportedDependency<IImportAssetsUseCase, ImportAssetsUseCase>();
            RegisterExportedDependency<IConsoleImporter, ConsoleImporter>();
            RegisterExportedDependency<IFileReader<string>, TextFileReader>();
            RegisterExportedDependency<ITextSplitter, TextSplitter>();
            RegisterExportedDependency<IInputParser<ImportAssetConsoleInput>, ImportAssetInputParser>();
            RegisterExportedDependency<IFactory<CreateAssetRequest, CsvAsset>, CreateAssetRequestFactory>();
            RegisterExportedDependency<ICalculateAssetAggregatesUseCase, CalculateAssetAggregatesUseCase>();
            RegisterExportedDependency<IAssetAggregator, EFAssetGateway>();
        }

        public override T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
