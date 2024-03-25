using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;

namespace OutfitO.Controllers
{
    public class PaymentController : Controller
    {
        IPaymentRepository paymentRepository;
        public PaymentController(IPaymentRepository paymentRepo)
        {
            paymentRepository = paymentRepo;
        }
        [Authorize("User")]
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [Authorize("User")]
        [HttpPost]
        public IActionResult Add(Payment payment)
        {
            if(ModelState.IsValid) {
                TempData["Payment"] = payment.Id;
                return RedirectToAction("Add", "Order");
            }
            return View("Add");
        }
    }
}
