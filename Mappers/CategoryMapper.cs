using FileForge.DTOs.Category;
using FileForge.Entities;
using FileForge.Enums;

namespace FileForge.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponseDto ToItem(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Status = category.Status.ToString(),
                ParentId = category.ParentId.GetValueOrDefault(),
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt.GetValueOrDefault()
            };
        }

        public static CategoryResponseDto ToCollection(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Status = category.Status.ToString(),
                ParentId = category.ParentId.GetValueOrDefault(),
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt.GetValueOrDefault()
            };
        }
    }
}
