using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutfitO.ViewModels;

namespace OutfitO.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> RoleManager)
        {
            roleManager = RoleManager;
        }

        public IActionResult AddRole()
        {
            return View("AddRole");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(RoleVM newRoleVM)
        {
            if (ModelState.IsValid == true)
            {
                IdentityRole roleModel = new IdentityRole()
                {
                    Name = newRoleVM.RoleName
                };
                IdentityResult rust = await roleManager.CreateAsync(roleModel);

            }
            return View("AddRole");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
