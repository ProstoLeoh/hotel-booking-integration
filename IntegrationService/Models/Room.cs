namespace IntegrationService.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomId { get; set; }
        public string HotelId { get; set; }
        public string Type { get; set; }  // single, double, suite
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
