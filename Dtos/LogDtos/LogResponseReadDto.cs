using CDI_Tool.Model;

namespace CDI_Tool.Dtos.LogDtos
{
    public class LogResponseReadDto
    {
        public List<Log> Data { get; set; }
        public int Total { get; set; }
        public AnalyticsInfo Analytics { get; set; }
    }
    public class AnalyticsInfo
    {
        public int TotalDecisions { get; set; }
        public int AppleDecisions { get; set; }
        public int GrandDecisions { get; set; }
        public int ApplePercentage { get; set; }
        public int GrandPercentage { get; set; }
        public List<TopUser> TopUsers { get; set; }
    }
    public class TopUser
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal TotalProfit { get; set; }
    }
}