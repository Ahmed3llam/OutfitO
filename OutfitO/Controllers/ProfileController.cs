using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OutfitO.Repository;
using OutfitO.Models;
using OutfitO.ViewModels;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace OutfitO.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        IUserRepository userRepository;
        private UserManager<User> userManager;
        ICategoryRepository categoryRepository;
        IOrderRepository orderRepository;
        IPaymentRepository PaymentRepository;
        IProductRepository ProductRepository;
        public ProfileController(IUserRepository _userRepository, UserManager<User> userManager,ICategoryRepository categoryRepo,IOrderRepository orderRepo,
            IPaymentRepository paymentRepo,IProductRepository productRepo)
        {
            userRepository = _userRepository;
            this.userManager = userManager;
            categoryRepository = categoryRepo;
            orderRepository = orderRepo;
            PaymentRepository = paymentRepo;
            ProductRepository = productRepo;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userRepository.GetUser(Id);
            if (User.Identity.IsAuthenticated)
            {
                var roles = await userManager.GetRolesAsync(user);
                bool isAdmin = roles.Contains("Admin");
                if (isAdmin)
                {
                    ViewData["Category"] = categoryRepository.GetAll();
                    ViewData["Products"] = ProductRepository.GetForUser(Id);
                    return View("Index", user);
                }
                else
                {
                    ViewData["Order"]=orderRepository.GetOrderForUSer(Id);
                    ViewData["Payment"]=PaymentRepository.GetPaymentForUSer(Id);
                    return View("IndexUser", user);
                }
            }
            return View("Error");
        }
        [HttpGet]
        public async Task<IActionResult> EditDataAsync(string id)
        {
            User user = userRepository.GetUser(id);
            UserDataVM vm = new UserDataVM()
            {
                Id = id,
                FirstName = user.FirstName,
                Lastname = user.Lastname,
                ProfileImage = user.ProfileImage,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Visa = user.Visa,
            };
            if (User.Identity.IsAuthenticated)
            {
                var roles = await userManager.GetRolesAsync(user);
                bool isAdmin = roles.Contains("Admin");
                if (isAdmin)
                {
                    return View("Edit", vm);
                }
                else
                {
                    return View("EditUser", vm);
                }
            }
            return View("Error");
        }
        [HttpPost]
        public IActionResult EditData(UserDataVM user)
        {
            if(ModelState.IsValid)
            {
                User updated = new User();
                updated = userRepository.GetUser(user.Id);
                updated.Id = user.Id;
                updated.FirstName = user.FirstName;
                updated.Lastname = user.Lastname;
                updated.PhoneNumber = user.PhoneNumber;
                updated.Email = user.Email;
                updated.Gender = user.Gender;
                updated.Visa = user.Visa;
                updated.Address = user.Address;
                userRepository.Update(updated);
                userRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", user);
        }
        [HttpGet]
        public IActionResult EditImage(string id)
        {
            User user = userRepository.GetUser(id);
            UserImageVm vm = new UserImageVm()
            {
                Id = id,
                ProfileImage = user.ProfileImage,
            };
            return PartialView("_EditImagePartial", vm);
        }
        [HttpPost]
        public IActionResult EditImage(UserImageVm user ,IFormFile ProfileImg)
        {
            if(ProfileImg != null && ProfileImg.Length > 0)
            {
                string oldImg = user.ProfileImage;
                string oldPath = $"wwwroot/Images/{oldImg}";
                int retryAttempts = 3;
                int retryDelayMs = 100; // milliseconds

                for (int i = 0; i < retryAttempts; i++)
                {
                    try
                    {
                        System.IO.File.Delete(oldPath);
                        break; // Break out of the loop if deletion is successful
                    }
                    catch (IOException)
                    {
                        // Log or handle the exception
                        // Wait for a short delay before retrying
                        Task.Delay(retryDelayMs).Wait();
                    }
                }
                string FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImg.FileName);
                string path = $"wwwroot/Images/{FileName}";
                FileStream fs = new FileStream(path, FileMode.Create);
                ProfileImg.CopyTo(fs);
                User updated = new User();
                updated = userRepository.GetUser(user.Id);
                updated.Id = user.Id;
                updated.ProfileImage = FileName;
                userRepository.Update(updated);
                userRepository.Save();
                return RedirectToAction("Index");
            }
            return PartialView("_EditImagePartial", user);
        }
        [HttpGet]
        public IActionResult EditPassword(string id)
        {
            User user = userRepository.GetUser(id);
            UserPasswordVM vm = new UserPasswordVM()
            {
                Id = id,
            };
            return PartialView("_EditPasswordPartial", vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(UserPasswordVM user)
        {
            if (ModelState.IsValid)
            {
                User updatedUser = userRepository.GetUser(user.Id);
                if (updatedUser != null)
                {
                    bool found = await userManager.CheckPasswordAsync(updatedUser, user.OldPassword);
                    if (found && user.NewPassword == user.ConfirmPassword)
                    {
                        var changePasswordResult = await userManager.ChangePasswordAsync(updatedUser, user.OldPassword, user.NewPassword);
                        if (changePasswordResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in changePasswordResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Input.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }
            ViewData["ModelState"] = ModelState;
            return PartialView("_EditPasswordPartial", user);
            //return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteUser(string id)
        {
            User user = userRepository.GetUser(id);
            return PartialView("_DeletePartial", user);
        }
        [HttpPost]
        public IActionResult DeleteUser(User user)
        {
            string oldImg = user.ProfileImage;
            string oldPath = $"wwwroot/Images/{oldImg}";
            int retryAttempts = 3;
            int retryDelayMs = 100; // milliseconds
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    System.IO.File.Delete(oldPath);
                    break;
                }
                catch (IOException)
                {
                    Task.Delay(retryDelayMs).Wait();
                }
            }
            userRepository.DeleteUser(user.Id);
            userRepository.Save();
            return RedirectToAction("Login","User");
        }
    }
}
