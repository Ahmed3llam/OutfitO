﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OutfitO.Models
{
    public class Order
    {
        public int Id { set; get; }
        public DateTime Date { set; get; }
        [Column(TypeName = "money")]
        public decimal Price { set; get; }
        [ForeignKey("User")]
        public string UserId { set; get; }
        [ForeignKey("payment")]
        public int? PaymentId { set; get; }

        // Navigation Properties
        public virtual Payment payment { set; get; }
        public virtual User User { set; get; }

        public virtual List<OrderItem> orderItems { set; get; } = new List<OrderItem>();
    }
}
