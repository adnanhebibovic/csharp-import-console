namespace csharp_import_console.models.metadata
{
    public class AnswerAlternative
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
    }

    public class Datum
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public string? Type { get; set; }
        public List<AnswerAlternative>? AnswerAlternatives { get; set; }
    }

    public class Status
    {
        public string? StatusCode { get; set; }
        public List<string>? Messages { get; set; }
    }

    public class MetadataResponse
    {
        public List<Datum>? Data { get; set; }
        public object? ErrorData { get; set; }
        public Status? Status { get; set; }
    }
}