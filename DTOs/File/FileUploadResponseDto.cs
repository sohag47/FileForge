namespace FileForge.DTOs.File
{
    public class FileUploadResponseDto
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty.ToString();
    }
}
