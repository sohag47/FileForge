namespace FileForge.DTOs.File
{
    public class FileUploadRequestDto
    {
        public List<IFormFile> Files { get; set; } = [];
        public string? FolderName { get; set; }
    }
}
