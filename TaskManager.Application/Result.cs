namespace TaskManager.Application
{
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public Result(
            bool success = true,
            string? message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
