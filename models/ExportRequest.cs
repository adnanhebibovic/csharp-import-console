namespace csharp_import_console.models
{
    public class Filter
    {
        public string? QuestionCode { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
    }

    public class SortCriteria
    {
        public string? QuestionCode { get; set; }
        public string? SortOrder { get; set; }
    }

    public class ExportRequest
    {
        public List<Filter>? Filters { get; set; }
        public List<string>? QuestionCodes { get; set; }
        public List<SortCriteria>? SortCriteria { get; set; }
        public bool ExportStackedData { get; set; }
        public bool IncludeGroupedAnswers { get; set; }
        public bool IncludeComputeVariables { get; set; }
        public bool IncludeInputVariables { get; set; }
        public bool IncludeMergedVariables { get; set; }
        public bool OutputEmptyStrings { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<string>? StackedDataBatches { get; set; }
        public string? DataExportFormat { get; set; }
    }


}