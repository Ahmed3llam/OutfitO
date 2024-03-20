using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutfitO.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        //public string? Sticker { get; set; }
        [ForeignKey("product")]
        public int ProductID { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }

        // Navigation Property
        public virtual Product product { get; set; }
        public virtual User user { get; set; }
    }
}
