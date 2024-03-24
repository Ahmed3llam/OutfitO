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
        [Authorize("Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize("Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize("Admin")]
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [Authorize("Admin")]
        [HttpPost]
        public IActionResult Delete(PromoCode promo)
        {
            return View();
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
