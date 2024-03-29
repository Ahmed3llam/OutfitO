using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutfitO.ViewModels
{
    public class CommentWithItsUser
    {
        public int Id { get; set; }

        [MinLength(5, ErrorMessage ="Comment Must Be More Than 5 Letters")]
        public string Body { get; set; }
        //public string? Sticker { get; set; }
        [ForeignKey("product")]
        public int ProductID { get; set; }
        [ForeignKey("user")]
        public string? UserId { get; set; }
    }
}
