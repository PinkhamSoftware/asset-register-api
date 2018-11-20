﻿using HomesEngland.Gateway.Sql.Postgres;
using Microsoft.EntityFrameworkCore;

namespace HomesEngland.Gateway.Migrations
{
    public class AssetRegisterContext:DbContext
    {
        private readonly string _databaseUrl;
        public AssetRegisterContext(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }
        /// <summary>
        /// Must be self contained for Entity Framework Command line tool to work
        /// </summary>
        public AssetRegisterContext()
        {
            _databaseUrl = "postgres://postgres:super-secret@localhost:5432/asset_register_api";
        }

        public DbSet<DapperAsset> Assets { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(new PostgresDatabaseConnectionStringFormatter().BuildConnectionStringFromUrl(_databaseUrl));
    }  
}
