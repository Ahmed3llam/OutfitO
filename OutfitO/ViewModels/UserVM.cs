using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OutfitO.ViewModels
{
    public class UserVM
    {
        [MinLength(3, ErrorMessage = "First Name Should be more than 3 letters")]
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Last Name Should be more than 3 letters")]
        public string Lastname { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }
        //[UniqePhone]
        [RegularExpression("^01[0125][0-9]{8}")]
        public string PhoneNumber { get; set; }
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender Should be Male Or Female")]
        public string Gender { get; set; }
        //[UniqueUser]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "password does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string? Visa { get; set; }
    }
}
