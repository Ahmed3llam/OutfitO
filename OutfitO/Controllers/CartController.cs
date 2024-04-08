using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OutfitO.Models;
using OutfitO.Repository;
using OutfitO.ViewModels;
using System.Security.Claims;

namespace OutfitO.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		int cartItemsCounter;
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
		[HttpGet]
		public IActionResult Index()
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			List<CartItem> cartItems = cartRepository.GetForUser(Userid);
			ViewData["Price"] = cartRepository.GetTotalPrice(Userid);
			ViewData["Count"] = cartItems.Count;
			//cartItemsCounter= cartItems.Count();
			//HttpContext.Session.SetInt32("cartItemsCounter", cartItemsCounter);
			return View("Index", cartItems);
		}
		[Authorize]
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
				int count = (int)HttpContext.Session.GetInt32("Count");
				HttpContext.Session.SetInt32("Count", ++count);
			}
			cartRepository.Save();
			return PartialView("_UserNavPartial");
		}

		[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult IncrementQuantity(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			CartItem cartItem = cartRepository.GetById(id, Userid);
			if (cartItem == null || cartItem.Quantity >= cartItem.Product.Stock)
			{
				return Json(new { message = "Not Allowed .. Max Quantity = " + cartItem.Product.Stock });
			}
			cartItem.Quantity += 1;
			cartRepository.Update(id, Userid, cartItem);
			cartRepository.Save();
            return Json(new { quantity = cartItem.Quantity, itemPrice = cartItem.Product.Price });
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DecreaseQuantity(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			CartItem cartItem = cartRepository.GetById(id, Userid);
			if (cartItem == null || cartItem.Quantity <= 1)
			{
                return Json(new { message = "Not Allowed .. Min Quantity = 1" });
            }
			cartItem.Quantity -= 1;
			cartRepository.Update(id, Userid, cartItem);
			cartRepository.Save();
            return Json(new { quantity = cartItem.Quantity, itemPrice = cartItem.Product.Price });
        }
        public IActionResult Delete(int id)
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			cartRepository.Delete(id, Userid);
			cartRepository.Save();
            cartItemsCounter--;
            int count = (int)HttpContext.Session.GetInt32("Count");
            HttpContext.Session.SetInt32("Count", --count);
            return RedirectToAction("Index");
		}

		public ActionResult DeleteAll()
		{
			var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			cartRepository.DeleteAll(Userid);
			cartRepository.Save();
            cartItemsCounter=0;
            HttpContext.Session.SetInt32("cartItemsCounter", cartItemsCounter);
            return RedirectToAction("Index","Cart");
		}
	}
}
