namespace wbs_api.Models;

public class Workstation
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Wing { get; set; } = string.Empty;
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsActive { get; set; } = true;
}