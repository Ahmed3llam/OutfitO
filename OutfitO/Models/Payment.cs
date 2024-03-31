namespace OutfitO.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; internal set; }
        // Add other properties as needed
    }
}
