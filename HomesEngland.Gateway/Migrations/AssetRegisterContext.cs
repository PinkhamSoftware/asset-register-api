using HomesEngland.Gateway.Sql.Postgres;
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
            _databaseUrl = System.Environment.GetEnvironmentVariable("DATABASE_URL");
        }

        public DbSet<AssetRegisterVersionEntity> AssetRegisterVersions { get; set; }
        public DbSet<AssetEntity> Assets { get; set; }
        public DbSet<AuthenticationTokenEntity> AuthenticationTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(new PostgresDatabaseConnectionStringFormatter().BuildConnectionStringFromUrl(_databaseUrl));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetRegisterVersionEntity>()
                .HasMany<AssetEntity>(b=> b.Assets)
                .WithOne();
        }
    }  
}
