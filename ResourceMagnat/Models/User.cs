using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ResourceMagnat.Data;

namespace ResourceMagnat.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Uid { get; set; }

    public Int64 Coins { get; set; }

    public int CoinsPerSecond { get; set; }

    public DateTime LastActive { get; set; }

    // public List<Building> Buildings { get; set; }

    public User(string uid)
    {
        Name = $"User {uid.Substring(0, 8)}";
        Uid = uid;
        Coins = 0;
        CoinsPerSecond = 0;
        LastActive = DateTime.Now;
    }

    public void UpdateCoinsPerSecond(MainDbContext context)
    {
        CoinsPerSecond = 0;
        var buildings = context.Buildings.Where(i => i.UserId == Id).ToList();
        foreach (var building in buildings)
        {
            CoinsPerSecond += building.GetCoinsPerSecond(context);
        }
    }

    public void UpdateCoins()
    {
        var secondsPassed = (DateTime.Now - LastActive).TotalSeconds;
        Coins += (Int64)(secondsPassed * CoinsPerSecond);
        LastActive = DateTime.Now;
    }
}