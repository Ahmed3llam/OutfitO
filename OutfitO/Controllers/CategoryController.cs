using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OutfitO.Models;
using OutfitO.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutfitO.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            categoryRepository = categoryRepo;
        }
        public IActionResult Index()
        {
            List<Category> categories = categoryRepository.GetAll();
            return View("Index",categories);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("AddCategory");
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
            return View("AddCategory");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category data = categoryRepository.Get(id);
            return View("EditCategory",data);
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
            return View("EditCategory", category);
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
