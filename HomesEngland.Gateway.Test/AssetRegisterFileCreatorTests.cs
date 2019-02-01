using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterFiles;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class AssetRegisterFileCreatorTests
    {
        private IAssetRegisterFileCreator _classUnderTest;

        [SetUp]
        public void Setup()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("SCHEDULE_DATABASE_URL");
            var context = new AssetRegisterScheduleContext(databaseUrl);

            context.Database.Migrate();
        }

    }
}
