using FileForge.DTOs;
using FileForge.DTOs.Category;

namespace FileForge.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAll();
    Task<CategoryResponseDto?> GetById(int id);
    Task<CategoryResponseDto> Create(CategoryCreateDto category);
    Task<CategoryResponseDto?> Update(int id, CategoryUpdateDto category);
    Task<bool> Delete(int id);
    Task<bool> BulkDelete(IEnumerable<int> ids);
    Task<bool> BulkCreate(IEnumerable<CategoryCreateDto> categories);
    Task<bool> BulkUpdate(IEnumerable<CategoryUpdateDto> categories);
    Task <IEnumerable<LookupDto>> Dropdown();
}
