using OutfitO.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class ProductWithCategoryList
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "First Name Should be more than 3 letters")]
        public string Title { get; set; }
        [MinLength(20, ErrorMessage = "First Name Should be more than 20 letters")]
        public string Description { get; set; }
        //[DataType(DataType.Upload)]
        public string Img { get; set; }
        //[Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        //[ForeignKey("User")]
        public string? UserID { get; set; }

        //[ForeignKey("Category")]
        public int CategoryId { get; set; }

    

        //public Category Category { get; set; }

        //public List<Category> Categories { get; set; }


    }
}
