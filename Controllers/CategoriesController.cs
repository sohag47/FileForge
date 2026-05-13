using FileForge.DTOs;
using FileForge.DTOs.Category;
using FileForge.Entities;
using FileForge.Entities.Base;
using FileForge.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileForge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;


    // GET: /categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
    {
        var categories = await _categoryService.GetAll();
        return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Ok("Category found successfully.", categories));
    }


    // GET: /categories/1
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
    {
        var response = await _categoryService.GetById(id);
        return Ok(ApiResponse<CategoryResponseDto>.Ok("Category found successfully.", response));
    }


    // POST: /categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> PostCategory([FromBody] CategoryCreateDto category)
    {
        var response = await _categoryService.Create(category);
        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.Id },
            ApiResponse<CategoryResponseDto>.Ok("Category created successfully.", response)
        );
    }


    // PUT: /categories/1
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> PutCategory(int id, [FromBody] CategoryUpdateDto category)
    {
        if (category.Id != id)
        {
            return BadRequest(ApiResponse<string>.Fail("Category parameter ID must match the URL ID."));
        }
        var response = await _categoryService.Update(id, category);
        if (response == null) return NotFound();

        return Ok(ApiResponse<CategoryResponseDto>.Ok("Category updated successfully.", response));
    }


    // DELETE: /categories/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id) 
    {
        var isDeleted = await _categoryService.Delete(id);
        return isDeleted ? NoContent() : NotFound();
    }

    // POST: /categories/bulk
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreateCategory([FromBody] List<CategoryCreateDto> categories)
    {
        if (categories == null || categories.Count == 0)
        {
            var errors = new Dictionary<string, string[]> { { "Categories", new[] { "Category list cannot be empty." } } };
            return UnprocessableEntity(ApiResponse<string>.Fail("Validation failed.", errors));
        }

        bool isCreated = await _categoryService.BulkCreate(categories);
        if (!isCreated)
        {
            BadRequest("Categories not created");
            
        }
        return Ok(ApiResponse<IEnumerable<LookupDto>>.Ok("Category Dropdown found"));
    }


    // PUT: /categories/bulk
    [HttpPut("bulk")]
    public async Task<IActionResult> BulkUpdateCategory([FromBody] List<CategoryUpdateDto> categories)
    {
        if (categories == null || categories.Count == 0)
        {
            var errors = new Dictionary<string, string[]> { { "Categories", new[] { "Category list cannot be empty." } } };
            return UnprocessableEntity(ApiResponse<string>.Fail("Validation failed.", errors));
        }

        var invalidCategories = categories.Where(c => c.Id <= 0).ToList();
        if (invalidCategories.Any())
        {
            var errors = new Dictionary<string, string[]> { { "Id", new[] { "All categories must have a valid ID for bulk update." } } };
            return UnprocessableEntity(ApiResponse<string>.Fail("Validation failed.", errors));
        }

        bool isUpdated = await _categoryService.BulkUpdate(categories);

        return isUpdated ? NoContent() : BadRequest("Categories not updated");
    }


    // DELETE: /categories/bulk
    [HttpDelete("bulk")]
    public async Task<IActionResult> BulkDeleteCategory([FromBody] List<int> ids)
    {
        
        if (ids == null || ids.Count == 0) return BadRequest("List cannot be empty.");
        var success = await _categoryService.BulkDelete(ids);
        return success ? NoContent() : NotFound("No matching categories found.");
    }


    // GET: /categories/lookup
    [HttpGet("lookup")]
    public async Task<ActionResult<IEnumerable<LookupDto>>> GetLookup()
    {
        var response = await _categoryService.Dropdown();
        return Ok(ApiResponse<IEnumerable<LookupDto>>.Ok("Category Dropdown found", response));
    }

    // POST: /categories/import
    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile file)
    {
        try
        {
            var result = await _categoryService.BulkImport(file);

            return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Ok("File processed successfully", result));
        }
        catch (Exception ex)
        {
            // Catch the guard clause exceptions
            return UnprocessableEntity(ApiResponse<string>.Fail(ex.Message, ex.ToString()));
        }
    

    }
}
