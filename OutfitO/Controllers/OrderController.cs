using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;
<<<<<<< HEAD
using System.Diagnostics;
using System.Security.Claims;

=======
using System.Security.Claims;
>>>>>>> B_Islam
namespace OutfitO.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository orderRepository;
<<<<<<< HEAD
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
=======
        public  OrderController(IOrderRepository orderRepo)
        {
            orderRepository = orderRepo;
        }

        //Order/Index
        public IActionResult Index()
        {
            List<Order> orders = orderRepository.GetAll();
            return View("Index", orders);
        }

        //Order/UserOrders
        public IActionResult UserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Order> orders = orderRepository.GetOrderForUSer(userId);
            return View("UserOrders", orders);
        }
        //Order/OrderDetails
        public IActionResult OrderDetails(int orderId)
        {
            List<OrderItem> orderItems = orderRepository.GetOrderItem(orderId);
            //return PartialView("OrderDetails", orderItems);
            return Json(orderItems);
        }

>>>>>>> B_Islam
    }
}
