using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OutfitO.Models;
using OutfitO.Repository;
using OutfitO.ViewModels;
using System.Security.Claims;

namespace OutfitO.Controllers
{
	public class CartController : Controller
	{
		ICartRepository cartRepository;
		IProductRepository productRepository;
		IOrderRepository orderRepository;
		IOrderItemsRepository orderItemsRepository;
		UserManager<User> userManager;

		public CartController(ICartRepository cartRepo, IProductRepository productRepo, UserManager<User> user, IOrderRepository orderRepo, IOrderItemsRepository orderItemsRepo)
		{
			cartRepository = cartRepo;
			productRepository = productRepo;
			orderRepository = orderRepo;
			orderItemsRepository = orderItemsRepo;
			userManager = user;
		}
		public IActionResult Index()
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			List<CartItem> cartItems = cartRepository.GetForUser(Userid);
			ViewData["Price"] = cartRepository.GetTotalPrice(Userid);
			ViewData["Count"] = cartItems.Count;

			return View("Index", cartItems);
		}

		public IActionResult AddToCart(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (cartRepository.GetById(id, Userid) != null)
			{
				cartRepository.GetById(id, Userid).Quantity++;
			}
			else
			{
				CartItem cartItem = new()
				{
					ProductID = id,
					UserID = Userid,
					Quantity = 1
				};
				cartRepository.Insert(cartItem);
			}
			cartRepository.Save();
			return RedirectToAction("Index", "Product");
		}
		[HttpPost]
		public IActionResult IncrementQuantity(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			CartItem cartItem = cartRepository.GetById(id, Userid);
			if (cartItem == null || cartItem.Quantity >= 10)
			{
				return NoContent();
			}
			cartItem.Quantity += 1;
			cartRepository.Update(id, Userid, cartItem);
			cartRepository.Save();
            return Json(new { quantity = cartItem.Quantity, itemPrice = cartItem.Product.Price });
        }
        [HttpPost]
        public IActionResult DecreaseQuantity(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			CartItem cartItem = cartRepository.GetById(id, Userid);
			if (cartItem == null || cartItem.Quantity <= 1)
			{
				return NoContent();
			}
			cartItem.Quantity -= 1;
			cartRepository.Update(id, Userid, cartItem);
			cartRepository.Save();
            return Json(new { quantity = cartItem.Quantity, itemPrice = cartItem.Product.Price });
        }
        [HttpPost]
        public IActionResult Delete(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			cartRepository.Delete(id, Userid);
			cartRepository.Save();
			return RedirectToAction("Cart", "Index");
		}

		public ActionResult DeleteAll()
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			cartRepository.DeleteAll(Userid);
			cartRepository.Save();
			return RedirectToAction("Cart", "Index");

			//return RedirectToAction("Details","Order");
		}
		//public IActionResult CheckOut()
		//{
		//    //promocode - action on promocontroller check for value of promocode then redirct to payment--
		//    //payment -> partialview tod isplay all payment methods for this user to select one of them the redirct to order
		//    //order -> (get userid from claims + gettotalprice using cartrepository then apply precentage of promocode) then redirct to orderitems
		//    //orderitem -> (get list of cartitem using cart repo  + update product for each item in cart list using product repo to decrease stock attr with quantity ) then redirct to delete cart items
		//    //delete cartitem -> delete cart then redirct to details for this order or history for all orders
		//    //details for this order -> display details for order
		//    var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
		//    List<CartItem> cartItems = cartRepository.GetForUser(Userid);
		//    decimal TPrice = cartRepository.GetTotalPrice(Userid);
		//    Order NewOrder = new Order()
		//    {
		//        Date = DateTime.Now,
		//        UserId = Userid,
		//        Price = TPrice,
		//        PaymentId = 1////
		//    };
		//    orderRepository.Insert(NewOrder);
		//    orderRepository.Save();
		//    foreach (CartItem item in cartItems)
		//    {
		//        Product product = productRepository.Get(item.ProductID);
		//        product.Stock -= item.Quantity;
		//        productRepository.Update(product);
		//        productRepository.Save();
		//        OrderItem newItem = new OrderItem()
		//        {
		//            ProductId = item.ProductID,
		//            OrderId = NewOrder.Id,
		//            Quantity = item.Quantity,
		//            Price = cartRepository.GetTotalPriceOfOneItem(item)
		//        };
		//        orderItemsRepository.Insert(newItem);
		//        cartRepository.Delete(item.Id);
		//        cartRepository.Save();
		//    }
		//    return RedirectToAction("Index", "Order");
		//}
	}
}
