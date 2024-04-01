using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }
    }
}
