using FileForge.Entities.Base;
using FileForge.Enums;

namespace FileForge.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public int? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = new List<Category>();

        public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    }
}
