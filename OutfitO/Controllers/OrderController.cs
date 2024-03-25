using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;
using System.Diagnostics;
using System.Security.Claims;

namespace OutfitO.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository orderRepository;
        public OrderController(IOrderRepository orderRepo)
        {
            orderRepository = orderRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details() {
            return View();
        }
        public IActionResult Add()
        {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order NewOrder = new Order()
            {
                Date = DateTime.Now,
                UserId = Userid,
                Price = (decimal)TempData["TPromoPrice"],
                PaymentId = (int)TempData["Payment"]
            };
            orderRepository.Insert(NewOrder);
            orderRepository.Save();
            TempData["Order"]=NewOrder.Id;
            return RedirectToAction("AddItems", "OrderItem");
        }
    }
}
