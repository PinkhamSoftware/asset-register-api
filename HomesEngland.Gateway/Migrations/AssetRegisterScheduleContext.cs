using HomesEngland.Gateway.Sql.Postgres;
using Microsoft.EntityFrameworkCore;

namespace HomesEngland.Gateway.Migrations
{

    public class AssetRegisterScheduleContext : DbContext
    {
        private readonly string _databaseUrl;
        public AssetRegisterScheduleContext(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        /// <summary>
        /// Must be self contained for Entity Framework Command line tool to work
        /// </summary>
        public AssetRegisterScheduleContext()
        {
            _databaseUrl = System.Environment.GetEnvironmentVariable("DATABASE_URL");
        }

        public DbSet<AssetRegisterFileEntity> AssetRegisterFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(new PostgresDatabaseConnectionStringFormatter().BuildConnectionStringFromUrl(_databaseUrl));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
