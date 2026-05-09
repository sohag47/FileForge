using FluentValidation;
using FileForge.DTOs.Category;

namespace FileForge.Validators.Category;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Slug)
            .MaximumLength(150).WithMessage("Category slug must not exceed 150 characters")
            .When(x => !string.IsNullOrEmpty(x.Slug));
    }
}
