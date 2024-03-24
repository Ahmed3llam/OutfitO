using Microsoft.AspNetCore.Authorization;
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
        public PromoCodeController(ICartRepository cartRepo,IPromoCodeRepository promoCodeRepo)
        {
            cartRepository = cartRepo;
            promoCodeRepository = promoCodeRepo;
        }
        //[Authorize("Admin")]
        public IActionResult Index(int page = 1)
        {
            int content = 3;
            int skip = (page - 1) * content;
            List<PromoCode> promoCodes = promoCodeRepository.GetSome(skip, content);
            int totalInstructors = promoCodeRepository.Count();
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = totalInstructors;
            return View("Index",promoCodes);
        }
        //[Authorize("Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_AddPartial");
        }
        //[Authorize("Admin")]
        [HttpPost]
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
        //[Authorize("Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
			PromoCode code = promoCodeRepository.Get(id);
			return PartialView("_DeletePartial", code);
		}
        //[Authorize("Admin")]
        [HttpPost]
        public IActionResult Delete(PromoCode promo)
        {
			promoCodeRepository.Delete(promo.Id);
			promoCodeRepository.Save();
			return RedirectToAction("Index", "PromoCode");
		}
        //[Authorize("User")]
        //[HttpGet]
        //public IActionResult CheckOut()
        //{
        //    return PartialView();
        //}
        [Authorize("User")]
        [HttpPost]
        public IActionResult CheckOut(PromoCode promo)
        {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            decimal TPrice = cartRepository.GetTotalPrice(Userid);
            decimal TPromoPrice = 0;
            /////
            if (promo.Code!=null)
            {
                var code = promoCodeRepository.GetPromoCode(promo.Code);
                if (code != null) {
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
            TempData["TPromoPrice"] = TPromoPrice;
            return RedirectToAction("Index", "Payment");
        }
    }
}
