using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OutfitO.Models;
using OutfitO.Repository;

using System.Diagnostics;
using System.Security.Claims;


namespace OutfitO.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository orderRepository;
        IUserRepository userRepository;
        IOrderItemsRepository orderItemsRepository;

        public OrderController(IOrderRepository orderRepo, IUserRepository userRepository , IOrderItemsRepository orderItemsRepository)
        {
            this.orderRepository = orderRepo;
            this.userRepository = userRepository;
            this.orderItemsRepository = orderItemsRepository;
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



        //Order/Index   this is For Admin
        public IActionResult Index(int page = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = userRepository.GetUser(userId);
            int content = 8;
            int skip = (page - 1) * content;
            List<Order> orders = orderRepository.GetSome(skip, content);
            int total = orderRepository.Count();
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;
            ViewData["User"] = user;
            return View("Index", orders);
        }

        //Order/History   this is For user
        public IActionResult History(int page = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = userRepository.GetUser(userId);
            int content = 8;
            int skip = (page - 1) * content;
            List<Order> orders = orderRepository.GetSomeOrdersForUser(userId , skip, content);
            int total = orderRepository.CountOrdersForUser(userId);
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;
            ViewData["User"] = user;
            return View("History", orders);
        }

        //Order/Details
        public IActionResult Details(int id)
        {
            List<OrderItem> orderItems = orderRepository.GetOrderItem(id);
            //if (orderItems.IsNullOrEmpty() || orderItems.Count == 0)
            //{
            //    orderItems = new List<OrderItem>();
              
            //}
            return PartialView("_DetailsPartial", orderItems);
        }

    }
}
