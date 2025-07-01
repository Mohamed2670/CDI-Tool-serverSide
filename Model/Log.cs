namespace CDI_Tool.Model
{
    public class Log : IEntity
    {
        public int Id { get; set; }

        public string Timestamp { get; set; }
        public string GuestName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string FirstInitial { get; set; }
        public string MRN { get; set; }
        public string Insurance { get; set; }
        public string Drugs { get; set; }
        public decimal TotalProfit { get; set; }
        public string Decision { get; set; }
        public string TransactionID { get; set; }
    }
}