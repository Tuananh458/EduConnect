namespace EduConnect.Helpers
{
    public class ApiResponse
    {
        public string Status { get; set; } = "success"; // "success" | "error"
        public string? Message { get; set; }
        public object? Data { get; set; }
        public object? Errors { get; set; }

        public static ApiResponse Success(object? data = null, string? message = null)
            => new() { Status = "success", Message = message, Data = data };

        public static ApiResponse Error(string message, object? errors = null)
            => new() { Status = "error", Message = message, Errors = errors };
    }
}
