namespace FileForge.Entities.Base
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public object? Errors { get; set; }

        // success response
        public static ApiResponse<T> Ok(string? message = null, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        // error response
        public static ApiResponse<T> Fail(string message, object? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

    }
}
