namespace Domain.Models
{
    public class RequestLog
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string RequestContent { get; set; } = string.Empty;
        public string? ResponseContent { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
