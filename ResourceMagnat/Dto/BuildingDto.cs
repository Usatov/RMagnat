using ResourceMagnat.Models;

namespace ResourceMagnat.Dto;

public class BuildingDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BuildingTypeId { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Level { get; set; }
}