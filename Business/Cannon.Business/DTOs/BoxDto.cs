namespace Cannon.Business.DTOs;

public class BoxDto
{
    public int Id { get; set; }
    public string BoxCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CurrentCount { get; set; }
}

public class CreateBoxDto
{
    public string BoxCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Capacity { get; set; }
}