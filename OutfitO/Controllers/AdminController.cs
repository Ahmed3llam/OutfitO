using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.ViewModels;

namespace OutfitO.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [Route("Admin/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserVM uservm, IFormFile ProfileImage)
        {
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                string FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
                string path = $"wwwroot/Images/{FileName}";
                FileStream fs = new FileStream(path, FileMode.Create);
                ProfileImage.CopyTo(fs);
                uservm.ProfileImage = FileName;
                //ModelState.SetModelValue("ProfileImage", new ValueProviderResult(FileName));
            }
            if (uservm.FirstName != null && uservm.Lastname != null && uservm.Password != null && uservm.ConfirmPassword != null &&
                uservm.PhoneNumber != null && uservm.Gender != null && uservm.Address != null)
            {
                User user = new User
                {
                    FirstName = uservm.FirstName,
                    Lastname = uservm.Lastname,
                    Email = uservm.Email,
                    PasswordHash = uservm.Password,
                    PhoneNumber = uservm.PhoneNumber,
                    Visa = uservm.Visa,
                    ProfileImage = uservm.ProfileImage,
                    Gender = uservm.Gender,
                    Address = uservm.Address,
                    UserName = uservm.FirstName + uservm.Lastname + Guid.NewGuid().ToString()
                };
                IdentityResult result = await userManager.CreateAsync(user, uservm.Password);
                if (result.Succeeded)
                {
                    IdentityResult resultRole = await userManager.AddToRoleAsync(user, "Admin");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", uservm);
            }
            return View("Register", uservm);
        }
    }
}
