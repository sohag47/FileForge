using FileForge.Enums;

namespace FileForge.DTOs.Category;

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public CategoryStatus? Status { get; set; }
}
