namespace IntegrationService.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingId { get; set; }
        public string RoomId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }  // PENDING, CONFIRMED, CANCELLED
        public DateTime CreatedAt { get; set; }
    }
}
