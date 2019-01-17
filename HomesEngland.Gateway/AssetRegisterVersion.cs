using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomesEngland.Domain;

namespace HomesEngland.Gateway
{
    [Table("assetregisterversions")]
    public class AssetRegisterVersion : IDatabaseEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("modifieddatetime")]
        public DateTime ModifiedDateTime { get; set; }
        
        public virtual IList<AssetEntity> Assets { get; set; }

        public AssetRegisterVersion() { }

        public AssetRegisterVersion(IAssetRegisterVersion assetRegisterVersion)
        {
            Id = assetRegisterVersion.Id;
            ModifiedDateTime = assetRegisterVersion.ModifiedDateTime;
        }
    }
}
