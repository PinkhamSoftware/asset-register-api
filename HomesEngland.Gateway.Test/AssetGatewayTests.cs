﻿using System.Threading.Tasks;
using System.Transactions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.Gateway.Sql.Postgres;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class AssetGatewayTests
    {
        private readonly IGateway<IAsset, int> _classUnderTest;
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;
        public AssetGatewayTests()
        {
            _databaseConnectionFactory = new PostgresDatabaseConnectionFactory(new PostgresDatabaseConnectionStringFormatter());
            var databaseUrl = System.Environment.GetEnvironmentVariable("DATABASE_URL");
            var connection = _databaseConnectionFactory.Create(databaseUrl);
            _classUnderTest = new SqlAssetGateway(connection);
            AssetRegisterContext assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [Test]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame()
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();
                var createdAsset = await _classUnderTest.CreateAsync(entity).ConfigureAwait(false);
                //act
                var readAsset = await _classUnderTest.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                //assert
                readAsset.AssetIsEqual(createdAsset.Id, entity);
                trans.Dispose();
            }
        }
    }
}
