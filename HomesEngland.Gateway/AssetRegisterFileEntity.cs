using System;
using System.ComponentModel.DataAnnotations.Schema;
using HomesEngland.Domain;

namespace HomesEngland.Gateway
{
    [Table("assetregisterfiles")]
    public class AssetRegisterFileEntity:IAssetRegisterFile
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }
        [Column("filename")]
        public string FileName { get; set; }
        [Column("modifieddatetime")]
        public DateTime ModifiedDateTime { get; set; }
    }
}
