using FileForge.DTOs.Category;
using FileForge.DTOs.File;
using FileForge.Entities.Base;
using FileForge.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileForge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IFileUploadService fileUpload) : ControllerBase
    {
        private readonly IFileUploadService _fileUpload = fileUpload;

        [HttpPost("upload")]
        public async Task<ActionResult<FileUploadResponseDto>> UploadImage(FileUploadRequestDto file)
        {
            FileUploadResponseDto response = await _fileUpload.Save(file);
            if (response.Status == false)
            {
                return UnprocessableEntity(ApiResponse<string>.Fail("Validation failed.", response));
            }
            return Ok(ApiResponse<FileUploadResponseDto>.Ok("File save successfully.", response));
        }


        [HttpDelete("fileDelete/{fileName}")]
        public async Task<ActionResult<FileUploadResponseDto>> DeleteImage(string fileName)
        {
            FileUploadResponseDto response = await _fileUpload.Remove(fileName);
            if (response.Status == false)
            {
                return UnprocessableEntity(ApiResponse<string>.Fail("Validation failed.", response));
            }
            return NoContent();

        }
    }
}
