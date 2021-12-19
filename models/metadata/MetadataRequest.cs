namespace csharp_import_console.models.metadata
{
    public class MetadataRequest
    {
        public bool IncludeStackedVariables { get; set; }
        public bool IncludeGroupedAnswers { get; set; }
        public bool IncludeComputeVariables { get; set; }
        public bool IncludeInputVariables { get; set; }
        public bool IncludeMergedVariables { get; set; }
    }
}