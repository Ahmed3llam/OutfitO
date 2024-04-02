using Microsoft.AspNetCore.Authorization;
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
            //string userId = TempData["User"] as string;
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (decimal.TryParse(TempData["TPrice"] as string, out decimal price))
            {
                if (int.TryParse(TempData["Payment"] as string, out int paymentId))
                {
                    Order NewOrder = new Order()
                    {
                        Date = DateTime.Now,
                        UserId = Userid,
                        Price = price,
                        PaymentId = paymentId 
                    };
                    orderRepository.Insert(NewOrder);
                    orderRepository.Save();
                    TempData["Order"] = NewOrder.Id.ToString();
                    return RedirectToAction("AddItems", "OrderItem");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
        //[Authorize(Roles ="Admin")]
        public IActionResult Index(int page = 1)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			User user = userRepository.GetUser(userId);
			int content = 8;
			int skip = (page - 1) * content;
			List<Order> orders = orderRepository.GetSomeOrders(skip, content);
			int total = orderRepository.Count();
			ViewData["Page"] = page;
			ViewData["content"] = content;
			ViewData["TotalItems"] = total;
			ViewData["User"] = user;
			return View("Index", orders);
		}

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
