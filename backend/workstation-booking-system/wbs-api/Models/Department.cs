using System.ComponentModel.DataAnnotations.Schema;

namespace wbs_api.Models;

[Table("department")]
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
