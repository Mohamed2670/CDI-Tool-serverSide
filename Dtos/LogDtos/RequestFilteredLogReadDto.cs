namespace CDI_Tool.Dtos.LogDtos
{
    public class RequestFilteredLogReadDto
    {
        public string? guestName { get; set; }
        public string? decision { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
    }
}