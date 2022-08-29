namespace ResourceMagnat.Dto;

public class UserDto
{
    public string Name { get; set; }

    public string Uid { get; set; }

    public Int64 Coins { get; set; }

    public int CoinsPerSecond { get; set; }

    public string SessionId { get; set; }
}