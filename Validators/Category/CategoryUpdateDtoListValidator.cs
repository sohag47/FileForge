using FluentValidation;
using FileForge.DTOs.Category;

namespace FileForge.Validators.Category;

public class CategoryUpdateDtoListValidator : AbstractValidator<List<CategoryUpdateDto>>
{
    public CategoryUpdateDtoListValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Category List is required and cannot be empty.");

        RuleForEach(x => x).SetValidator(new CategoryUpdateDtoValidator());
    }
}
