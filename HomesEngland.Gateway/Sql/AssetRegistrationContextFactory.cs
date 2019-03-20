using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomesEngland.Gateway.Sql
{
    public class AssetRegistrationContextFactory
    {
        public AssetRegisterContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<AssetRegisterContext>();
            options.UseSqlServer(connectionString);
            return new AssetRegisterContext(options.Options);
        }
    }
}
