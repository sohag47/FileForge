using FileForge.DTOs.File;
using FluentValidation;

namespace FileForge.Validators
{
    public class FileUploadDtoValidator : AbstractValidator<FileUploadRequestDto>
    {
        private readonly string[] AllowedExtensions =
        [
            ".jpg",
            ".jpeg",
            ".png",
            ".pdf",
            ".csv",
            ".xlsx"
        ];

        public FileUploadDtoValidator()
        {
            RuleFor(x => x.Files)
                .NotEmpty()
                .WithMessage("Files are required.");

            RuleFor(x => x.Files)
                .Must(BeUniqueFiles)
                .WithMessage("Duplicate files are not allowed.");

            RuleForEach(x => x.Files)
                .Must(HaveValidExtension)
                .WithMessage("Invalid file type.");

            RuleForEach(x => x.Files)
                .Must(x => x.Length <= 5 * 1024 * 1024)
                .WithMessage("File size exceeded.");
        }

        private bool BeUniqueFiles(List<IFormFile> files)
        {
            return files
                .Select(x => x.FileName.ToLower().Trim())
                .Distinct()
                .Count() == files.Count;
        }

        private bool HaveValidExtension(IFormFile file)
        {
            string extension = Path
                .GetExtension(file.FileName)
                .ToLower();

            return AllowedExtensions.Contains(extension);
        }
    }
}
