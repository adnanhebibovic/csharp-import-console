namespace csharp_import_console.models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Data
    {
        public string? RequestId { get; set; }
        public string? Status { get; set; }
        public DateTime DateStarted { get; set; }
        public List<string>? Files { get; set; }
    }

    public class Status
    {
        public string? StatusCode { get; set; }
        public List<object>? Messages { get; set; }
    }

    public class ExportResponse
    {
        public Data? Data { get; set; }
        public object? ErrorData { get; set; }
        public Status? Status { get; set; }
    }


}