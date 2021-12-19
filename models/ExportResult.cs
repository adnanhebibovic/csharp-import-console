using System.Text.Json.Serialization;

namespace csharp_import_console.models
{
    public class CaseData
    {
        public string? RespondentID { get; set; }
        public string? ResponseDate { get; set; }
        public string? Weight { get; set; }

        [JsonPropertyName("Bathroom floor txt")]
        public string? BathroomFloorTxt { get; set; }

        [JsonPropertyName("Bathroom floor")]
        public string? BathroomFloor { get; set; }

        [JsonPropertyName("Bedside tables txt")]
        public string? BedsideTablesTxt { get; set; }

        [JsonPropertyName("Bedside tables")]
        public string? BedsideTables { get; set; }

        [JsonPropertyName("Blankets txt")]
        public string? BlanketsTxt { get; set; }
        public string? Blankets { get; set; }

        [JsonPropertyName("Carpet txt")]
        public string? CarpetTxt { get; set; }
        public string? Carpet { get; set; }
        public List<string>? City { get; set; }
        public string? Comments { get; set; }

        [JsonPropertyName("Complaints per respondent")]
        public string? ComplaintsPerRespondent { get; set; }

        [JsonPropertyName("Entertainment txt")]
        public string? EntertainmentTxt { get; set; }
        public string? Entertainment { get; set; }

        [JsonPropertyName("Hygiene products txt")]
        public string? HygieneProductsTxt { get; set; }

        [JsonPropertyName("Hygiene products")]
        public string? HygieneProducts { get; set; }

        [JsonPropertyName("Minibar txt")]
        public string? MinibarTxt { get; set; }
        public string? Minibar { get; set; }

        [JsonPropertyName("Mirror txt")]
        public string? MirrorTxt { get; set; }
        public string? Mirror { get; set; }
        public List<string>? Person { get; set; }

        [JsonPropertyName("Pillows txt")]
        public string? PillowsTxt { get; set; }
        public string? Pillows { get; set; }

        [JsonPropertyName("Seat group txt")]
        public string? SeatGroupTxt { get; set; }

        [JsonPropertyName("Seat group")]
        public string? SeatGroup { get; set; }

        [JsonPropertyName("Sideboard txt")]
        public string? SideboardTxt { get; set; }
        public string? Sideboard { get; set; }

        [JsonPropertyName("Towels and Bathrobes txt")]
        public string? TowelsAndBathrobesTxt { get; set; }

        [JsonPropertyName("Towels and Bathrobes")]
        public string? TowelsAndBathrobes { get; set; }

        [JsonPropertyName("Washbasin txt")]
        public string? WashbasinTxt { get; set; }
        public string? Washbasin { get; set; }

        [JsonPropertyName("Waste basket txt")]
        public string? WasteBasketTxt { get; set; }

        [JsonPropertyName("Waste basket")]
        public string? WasteBasket { get; set; }

        [JsonPropertyName("Window and Curtains txt")]
        public string? WindowAndCurtainsTxt { get; set; }

        [JsonPropertyName("Window and Curtains")]
        public string? WindowAndCurtains { get; set; }

        [JsonPropertyName("Writing table txt")]
        public string? WritingTableTxt { get; set; }

        [JsonPropertyName("Writing table")]
        public string? WritingTable { get; set; }
    }

    public class AnswerAlternative
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public object? Score { get; set; }
        public bool IsGrouped { get; set; }
    }

    public class MetaData
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public string? Type { get; set; }
        public bool IsMerged { get; set; }
        public bool IsComputed { get; set; }
        public bool IsInput { get; set; }
        public List<AnswerAlternative>? AnswerAlternatives { get; set; }
    }

    public class ImportContext
    {
        public int ImportType { get; set; }
        public int ImportSource { get; set; }
        public bool StackedData { get; set; }
        public bool OverwriteMetadata { get; set; }
        public bool UpdateData { get; set; }
        public string? DateFormat { get; set; }
    }

    public class ExportResult
    {
        public List<CaseData>? CaseData { get; set; }
        public List<MetaData>? MetaData { get; set; }
        public ImportContext? ImportContext { get; set; }
    }


}