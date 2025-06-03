namespace Application.Common
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> SuccessResult(T data, string? message = null)
        {
            return new OperationResult<T> { Success = true, Data = data, Message = message };
        }

        public static OperationResult<T> Failure(string error, string? message = null)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message ?? error,
                Errors = new List<string> { error }
            };
        }

        public static OperationResult<T> Failure(List<string> errors, string? message = null)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
