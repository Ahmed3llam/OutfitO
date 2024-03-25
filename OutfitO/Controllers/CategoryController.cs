using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OutfitO.Models;
using OutfitO.Repository;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutfitO.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        IUserRepository userRepository;
        public CategoryController(ICategoryRepository categoryRepo, IUserRepository userRepo)
        {
            categoryRepository = categoryRepo;
            userRepository = userRepo;
        }
        public IActionResult Index(int page = 1)
        {
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int content = 8;
            int skip = (page - 1) * content;
            List<Category> categories = categoryRepository.GetSome(skip, content);
            int total = categoryRepository.Count();
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;
            ViewData["User"] = user;
            return View("Index",categories);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_AddPartial");
        }
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (category.Title!=null)
            {
                categoryRepository.Insert(category);
                categoryRepository.Save();
                return RedirectToAction("Index","Category");

            }
            return PartialView("_AddPartial");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category data = categoryRepository.Get(id);
            return PartialView("_EditPartial",data);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Title!=null)
            {
                categoryRepository.Update(category);
                categoryRepository.Save();;
                return RedirectToAction("Index", "Category");
            }
            return PartialView("_EditPartial", category);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = categoryRepository.Get(id);
            return PartialView("_DeleteCategoryPartial",category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            if (category == null)
            {
                return NotFound();
            }
            categoryRepository.Delete(category.Id);
            categoryRepository.Save();
            return RedirectToAction("Index", "Category");
        }
    }
}
