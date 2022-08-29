using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceMagnat.Models;

public class BuildingType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public int InitionalCost { get; set; }

    public int InitionalCoins { get; set; }

    public int CoinsPerSecond(int level)
    {
        return InitionalCoins * level;
    }

    public int CoinsCost(int level)
    {
        return (int)(InitionalCost * (1 + 0.1 * (level - 1)));
    }
}