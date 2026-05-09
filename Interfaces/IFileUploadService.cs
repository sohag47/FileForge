using FileForge.DTOs.File;

namespace FileForge.Interfaces
{
    public interface IFileUploadService
    {
        Task<FileUploadResponseDto> Save(FileUploadRequestDto request);
        Task<FileUploadResponseDto> Remove(string fileName);
    }
}
