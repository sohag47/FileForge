using FileForge.DTOs.File;
using FileForge.Interfaces;

namespace FileForge.Services
{
    public class FileUploadService(IConfiguration configuration) : IFileUploadService
    {
        private readonly string _uploadFolder = configuration["FileStorage:ImageFolder"] ?? "DefaultUploads";

        public async Task<FileUploadResponseDto> Save(FileUploadRequestDto request)
        {
            var file = request.Files?.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                return new FileUploadResponseDto
                {
                    FileName = null,
                    FilePath = null,
                    Message = "File is required",
                    Status = false,

                };
            }
            // 2. Define allowed extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // accept extension
            if (!allowedExtensions.Contains(extension))
            {
                return new FileUploadResponseDto
                {
                    FileName = file.FileName,
                    FilePath = null,
                    Message = "Invalid file type",
                    Status = false,

                };
            }


            string rootPath = Directory.GetCurrentDirectory();
            string path = Path.Combine(rootPath, _uploadFolder);

            // if folder not exit than create folder
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // generate file name
            string fileName = $"{Guid.NewGuid()}-{file.FileName}";
            string fullPath = Path.Combine(path, fileName);

            // 5. Save the file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return new FileUploadResponseDto
            {
                FileName = fileName,
                FilePath = fullPath,
                Message = "File Upload Successfully",
                Status = true,

            };
        }


        public async Task<FileUploadResponseDto> Remove(string fileName)
        {
            // 1. Get the folder path from configuration
            string rootPath = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine(rootPath, _uploadFolder);

            // 2. Combine with the filename to get the absolute path
            string fullPath = Path.Combine(folderPath, fileName);

            try
            {
                // 3. Safety Check: Does the file actually exist?
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return new FileUploadResponseDto
                    {
                        FileName = null,
                        FilePath = null,
                        Message = "File deleted Successfully",
                        Status = true,

                    };
                }
                else
                {
                    //return "File not found on the server.";
                    return new FileUploadResponseDto
                    {
                        FileName = fileName,
                        FilePath = null,
                        Message = "File not found on the server",
                        Status = false,

                    };
                }
            }
            catch (IOException ioEx)
            {
                // This happens if the file is being used by another process
                return new FileUploadResponseDto
                {
                    FileName = fileName,
                    FilePath = fullPath,
                    Message = $"Error deleting file: {ioEx.Message}",
                    Status = false,

                };
            }
        }
    }
}
