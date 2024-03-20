using System.ComponentModel.DataAnnotations.Schema;

namespace OutfitO.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
