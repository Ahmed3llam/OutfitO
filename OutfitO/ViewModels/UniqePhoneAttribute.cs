using OutfitO.Models;
using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class UniqePhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            User FormData = validationContext.ObjectInstance as User;
            string PhoneUser = value?.ToString();
            OutfitoContext context = new OutfitoContext();
            User user = context.User.FirstOrDefault(c => c.PhoneNumber == PhoneUser);
            if (user == null || user.Id == FormData.Id)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Use Other Phone Number");
        }
    }
}
