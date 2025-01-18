namespace TaskManager.Application
{
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Response { get; set; }

        public Result(bool success = true, string? message = null, object? response = null)
        {
            Success = success;
            Message = message;
            Response = response;
        }
    }
}
