using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class UserDataVM
    {
        public string Id { get; set; }
        [MinLength(3, ErrorMessage = "First Name Should be more than 3 letters")]
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Last Name Should be more than 3 letters")]
        public string Lastname { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }
        [RegularExpression("^01[0125][0-9]{8}")]
        public string PhoneNumber { get; set; }
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender Should be Male Or Female")]
        public string Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Visa { get; set; }
    }
}
