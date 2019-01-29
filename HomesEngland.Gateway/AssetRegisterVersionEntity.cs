using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HomesEngland.Domain;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;

namespace HomesEngland.Gateway
{
    [Table("assetregisterversions")]
    public class AssetRegisterVersionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("modifieddatetime")]
        public DateTime ModifiedDateTime { get; set; }
        
        public virtual IList<AssetEntity> Assets { get; set; }

        public AssetRegisterVersionEntity() { }

        public AssetRegisterVersionEntity(IAssetRegisterVersion assetRegisterVersion)
        {
            Id = assetRegisterVersion.Id;
            ModifiedDateTime = assetRegisterVersion.ModifiedDateTime;
            Assets = assetRegisterVersion.Assets?.Select(s=> new AssetEntity(s)).ToList();
        }
    }
}
