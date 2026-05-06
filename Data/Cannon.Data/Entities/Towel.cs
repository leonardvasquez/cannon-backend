namespace Cannon.Data.Entities;

public class Towel
{
    public int Id { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public Enums.TowelStatus Status { get; set; }
    public int? BoxId { get; set; }
    public Box? Box { get; set; }
    public bool IsActive { get; set; } = true;
}