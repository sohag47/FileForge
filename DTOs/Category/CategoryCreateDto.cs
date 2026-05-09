using FileForge.Enums;
using System.ComponentModel.DataAnnotations;
namespace FileForge.DTOs.Category;

public class CategoryCreateDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public CategoryStatus Status { get; set; } 
}
