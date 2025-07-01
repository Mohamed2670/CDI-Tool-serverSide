using System.Text.Json.Serialization;
namespace CDI_Tool.Dtos.LogDtos
{
    public class LogCreateDto
    {
        [JsonPropertyName("Guest Name")]
        public string GuestName { get; set; } = "";

        [JsonPropertyName("Last Name")]
        public string LastName { get; set; } = "";

        public string DOB { get; set; } = ""; // or DateTime if your frontend sends ISO

        [JsonPropertyName("First Initial")]
        public string FirstInitial { get; set; } = "";

        public string MRN { get; set; } = "";

        public string Insurance { get; set; } = "";

        [JsonPropertyName("Selected Drugs")]
        public string SelectedDrugs { get; set; } = "";

        [JsonPropertyName("Total Profit")]
        public string TotalProfit { get; set; } = "";

        [JsonPropertyName("Final Decision")]
        public string Decision { get; set; } = "";

        [JsonPropertyName("Transaction ID")]
        public string TransactionID { get; set; } = "";

        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o"); // ISO 8601
    }
}