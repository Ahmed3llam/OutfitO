using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;
using OutfitO.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace OutfitO.Controllers
{
	public class PromoCodeController : Controller
	{
		ICartRepository cartRepository;
		IPromoCodeRepository promoCodeRepository;
		IUserRepository userRepository;
		public PromoCodeController(ICartRepository cartRepo, IPromoCodeRepository promoCodeRepo, IUserRepository userRepo)
		{
			cartRepository = cartRepo;
			promoCodeRepository = promoCodeRepo;
			userRepository = userRepo;
		}
        [Authorize(Roles = "Admin")]
        public IActionResult Index(int page = 1)
		{
			User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
			int content = 8;
			int skip = (page - 1) * content;
			List<PromoCode> promoCodes = promoCodeRepository.GetSome(skip, content);
			int total = promoCodeRepository.Count();
			ViewData["Page"] = page;
			ViewData["content"] = content;
			ViewData["TotalItems"] = total;
			ViewData["User"] = user;
			return View("Index", promoCodes);
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
		public IActionResult Add()
		{
			return PartialView("_AddPartial");
		}
		
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(PromoCode promoCode)
		{
			if (ModelState.IsValid)
			{
				promoCodeRepository.Insert(promoCode);
				promoCodeRepository.Save();
				return RedirectToAction("Index", "PromoCode");
			}
			return PartialView("_AddPartial", promoCode);
		}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
		{
			PromoCode code = promoCodeRepository.Get(id);
			return PartialView("_DeletePartial", code);
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(PromoCode promo)
		{
			promoCodeRepository.Delete(promo.Id);
			promoCodeRepository.Save();
			return RedirectToAction("Index", "PromoCode");
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public IActionResult CheckOut(string promo)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			decimal TPrice = cartRepository.GetTotalPrice(Userid);
			decimal TPromoPrice = 0;

			if (promo != null)
			{
				var code = promoCodeRepository.GetPromoCode(promo);
				if (code != null)
				{
					decimal PromoPrice = TPrice * code.Percentage / 100;
					TPromoPrice = TPrice - PromoPrice;
				}
				else
				{
					return NotFound();
				}
			}
			else
			{
				TPromoPrice = TPrice;
			}
            HttpContext.Session.SetString("TPromoPrice", TPromoPrice.ToString("0.00"));

            return RedirectToAction("Index", "CheckOut");
		}

	}
}
