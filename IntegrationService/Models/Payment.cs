namespace IntegrationService.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentId { get; set; }
        public string BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }  // SUCCESS, FAILED
        public DateTime PaymentDate { get; set; }
    }
}
