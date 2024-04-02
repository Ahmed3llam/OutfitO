using OutfitO.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace OutfitO.ViewModels
{
    public class ProductWithCategoryList
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Title Should be more than 3 letters")]
        public string Title { get; set; }
        [MinLength(20, ErrorMessage = "Description Should be more than 20 letters")]
        public string Description { get; set; }
        [RegularExpression(@"^.+\.(jpg|png)$", ErrorMessage = "Image must be png or jpg")]
        public string Img { get; set; }
        public decimal Price { get; set; }
        [Range(1,int.MaxValue, ErrorMessage = "Count Should be at least 1")]
        [Display(Name = "Count In Stock")]
        public int Stock { get; set; }
        [Display(Name = "User")]
        public string? UserID { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
