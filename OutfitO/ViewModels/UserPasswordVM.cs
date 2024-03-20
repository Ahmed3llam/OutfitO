using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class UserPasswordVM
    {
        public string Id { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Old Password")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]

        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "password does not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; }
    }
}
