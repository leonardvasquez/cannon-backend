namespace Cannon.Data.Entities;

public class Box
{
    public int Id { get; set; }
    public string BoxCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public Enums.BoxStatus Status { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Towel> Towels { get; set; } = new List<Towel>();
}