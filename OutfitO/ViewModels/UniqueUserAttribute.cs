using Microsoft.EntityFrameworkCore;
using OutfitO.Models;
using System.ComponentModel.DataAnnotations;

namespace OutfitO.ViewModels
{
    public class UniqueUserAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            User FormData = validationContext.ObjectInstance as User;
            string EmailUser = value?.ToString();
            OutfitoContext context = new OutfitoContext();
            User user = context.User.FirstOrDefault(c => c.Email == EmailUser);
            if (user == null || user.Id == FormData.Id)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Use Other Email");
        }
    }
}
