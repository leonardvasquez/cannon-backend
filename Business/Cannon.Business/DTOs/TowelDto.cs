namespace Cannon.Business.DTOs;

public class TowelDto
{
    public int Id { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? BoxId { get; set; }
}