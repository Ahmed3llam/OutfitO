using System.ComponentModel.DataAnnotations.Schema;

namespace OutfitO.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }
        public string VisaNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
