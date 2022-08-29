using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ResourceMagnat.Data;

namespace ResourceMagnat.Models;

//public enum BuildingTypeEnum
//{
//    Pipe = 1,
//    Pump = 2,
//    Factory = 3
//}

public class Building
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("BuildingUserId")]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("BuildingTypeId")]
    public int BuildingTypeId { get; set; }

    // public BuildingType BuildingType { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Level { get; set; }

    public int GetCoinsPerSecond(MainDbContext context)
    {
        var buildingType = context.BuildingTypes.First(i => i.Id == BuildingTypeId);
        return buildingType.CoinsPerSecond(Level);
    }

    public int GetCoinsCost(MainDbContext context)
    {
        var buildingType = context.BuildingTypes.First(i => i.Id == BuildingTypeId);
        return buildingType.CoinsCost(Level);
    }

    //public int GetInitionalCost(MainDbContext context)
    //{
    //    var buildingType = context.BuildingTypes.First(i => i.Id == BuildingTypeId);
    //    return buildingType.InitionalCost;
    //}
}