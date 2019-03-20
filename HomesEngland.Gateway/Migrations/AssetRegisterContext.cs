using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Linq;

namespace HomesEngland.Gateway.Migrations
{
    public class AssetRegisterContext:DbContext
    {
        public AssetRegisterContext(DbContextOptions<AssetRegisterContext> options)
            : base(options)
        { }

        public DbSet<AssetRegisterVersionEntity> AssetRegisterVersions { get; set; }
        public DbSet<AssetEntity> Assets { get; set; }
        public DbSet<AuthenticationTokenEntity> AuthenticationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                if (property.Relational().ColumnType == null)
                    property.Relational().ColumnType = "decimal(13,4)";
            }

            modelBuilder.Entity<AssetRegisterVersionEntity>()
                .HasMany<AssetEntity>(b=> b.Assets)
                .WithOne();
        }
    }  
}
