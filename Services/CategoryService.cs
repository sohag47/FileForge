using FileForge.Data;
using FileForge.DTOs;
using FileForge.DTOs.Category;
using FileForge.Entities;
using FileForge.Interfaces;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System.Formats.Asn1;
using System.Globalization;

namespace FileForge.Services;

public class CategoryService(ApplicationDbContext context): ICategoryService
{
    protected readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<CategoryResponseDto>> GetAll()
    {
        return await _context.Categories.AsNoTracking()
        .Select(item => new CategoryResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Slug = item.Slug,
            Status = item.Status,
            ParentId = item.ParentId.GetValueOrDefault(),
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt.GetValueOrDefault()
        })
        .ToListAsync();
    }

    public async Task<CategoryResponseDto?> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Status = category.Status,
            ParentId = category.ParentId.GetValueOrDefault(),
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt.GetValueOrDefault()
        };
    }

    public async Task<CategoryResponseDto> Create(CategoryCreateDto categoryDto)
    {
        var category = new Entities.Category
        {
            Name = categoryDto.Name,
            Slug = categoryDto.Slug,
            ParentId = categoryDto.ParentId,
            Status = categoryDto.Status
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Status = category.Status,
            ParentId = category.ParentId.GetValueOrDefault(),
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt.GetValueOrDefault()
        };
    }

    public async Task<CategoryResponseDto?> Update(int id, CategoryUpdateDto categoryDto)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null) return null;

        if (!string.IsNullOrWhiteSpace(categoryDto.Name))
        {
            existingCategory.Name = categoryDto.Name;
        }

        if (!string.IsNullOrWhiteSpace(categoryDto.Slug))
        {
            existingCategory.Slug = categoryDto.Slug;
        }

        existingCategory.ParentId = categoryDto.ParentId ?? existingCategory.ParentId;

        if (categoryDto.Status.HasValue && Enum.IsDefined(typeof(Enums.CategoryStatus), categoryDto.Status.Value))
        {
            existingCategory.Status = categoryDto.Status.Value;
        }

        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = existingCategory.Id,
            Name = existingCategory.Name,
            Slug = existingCategory.Slug,
            Status = existingCategory.Status,
            ParentId = existingCategory.ParentId.GetValueOrDefault(),
            CreatedAt = existingCategory.CreatedAt,
            UpdatedAt = existingCategory.UpdatedAt.GetValueOrDefault()
        };
    }

    public async Task<bool> Delete(int id)
    {
        var deletedCount = await _context.Categories
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return deletedCount > 0;

    }
    
    public async Task<bool> BulkCreate(IEnumerable<CategoryCreateDto> categoryDto)
    {
        var newCategories = categoryDto.Select(d => new Entities.Category
        {
            Name = d.Name,
            Slug = d.Slug,
            Status = d.Status,
            ParentId=d.ParentId,
        }).ToList();

        await _context.Categories.AddRangeAsync(newCategories);
        await _context.SaveChangesAsync();

        return newCategories.Count > 0;
    }

    public async Task<bool> BulkUpdate(IEnumerable<CategoryUpdateDto> categoryDto)
    {
        var categoryIds = categoryDto.Where(d => d.Id > 0).Select(d => d.Id).ToList();
        if (!categoryIds.Any()) return false;

        var existingCategories = await _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
        if (!existingCategories.Any()) return false;

        foreach (var category in existingCategories)
        {
            var updateDto = categoryDto.FirstOrDefault(d => d.Id == category.Id);
            if (updateDto != null)
            {
                if (!string.IsNullOrWhiteSpace(updateDto.Name))
                {
                    category.Name = updateDto.Name;
                }
                if (!string.IsNullOrWhiteSpace(updateDto.Slug))
                {
                    category.Slug = updateDto.Slug;
                }

                category.ParentId = updateDto.ParentId ?? category.ParentId;

                if (updateDto.Status.HasValue && Enum.IsDefined(typeof(Enums.CategoryStatus), updateDto.Status.Value))
                {
                    category.Status = updateDto.Status.Value;
                }
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BulkDelete(IEnumerable<int> ids)
    {
        int rowsAffected = await _context.Categories
        .Where(c => ids.Contains(c.Id))
        .ExecuteDeleteAsync();

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<LookupDto>> Dropdown()
    {
        return await _context.Categories
        .AsNoTracking()
        .Select(c => new LookupDto
        {
            Value = c.Id,
            Label = c.Name
        })
        .ToListAsync();
    }

    public async Task<IEnumerable<CategoryResponseDto>> BulkImport(IFormFile file)
    {
        // 1. Guard Clauses (Pre-validation)
        if (file == null || file.Length == 0)
            throw new Exception("File is empty or missing.");

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".xlsx")
            throw new Exception("Unsupported file format. Please upload an .xlsx file.");

        // 2. Parse File
        List<Category> rawData;
        using (var stream = file.OpenReadStream())
        {
            // QueryAsync is excellent for performance with MiniExcel
            var rows = await stream.QueryAsync<Category>();
            rawData = rows.ToList();
        }

        // 3. Row Count Check
        if (rawData.Count == 0)
            throw new Exception("The file contains no data rows.");

        // 4. Optimized Single-Pass Processing
        var responseDtos = new List<CategoryResponseDto>();
        var entitiesToSave = new List<Entities.Category>();

        foreach (var item in rawData)
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(item.Name);

            // Map to Response DTO (Always return to user so they see what happened)
            responseDtos.Add(new CategoryResponseDto
            {
                Id = item.Id,
                Name = item.Name ?? "N/A",
                Slug = item.Slug ?? string.Empty,
                Status = item.Status,
                ParentId = item.ParentId.GetValueOrDefault(),
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt.GetValueOrDefault()
            });

            // If valid, prepare for database insert
            if (isNameValid)
            {
                entitiesToSave.Add(new Entities.Category
                {
                    Name = item.Name!,
                    // Ensure Slug is URL friendly even if Excel data is messy
                    Slug = string.IsNullOrWhiteSpace(item.Slug)
                           ? item.Name!.ToLower().Replace(" ", "-")
                           : item.Slug,
                    ParentId = item.ParentId,
                    Status = item.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        // 5. Bulk Database Operation
        if (entitiesToSave.Count > 0)
        {
            await _context.Categories.AddRangeAsync(entitiesToSave);
            await _context.SaveChangesAsync();
        }

        return responseDtos;
    }
}
