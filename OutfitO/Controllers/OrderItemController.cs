using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;
using System.Security.Claims;

namespace OutfitO.Controllers
{
    public class OrderItemController : Controller
    {
        IOrderRepository orderRepository;
        IOrderItemsRepository orderItemsRepository;
        ICartRepository cartRepository;
        IProductRepository productRepository;
        public OrderItemController(IOrderRepository orderRepo, IOrderItemsRepository orderItemsRepo, ICartRepository cartRepo, IProductRepository productRepo)
        {
            orderRepository = orderRepo;
            orderItemsRepository = orderItemsRepo;
            cartRepository = cartRepo;
            productRepository = productRepo;
        }
        public IActionResult OrderItem() {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<CartItem> cartItems = cartRepository.GetForUser(Userid);
            foreach (CartItem item in cartItems)
            {
                Product product = productRepository.Get(item.ProductID);
                product.Stock -= item.Quantity;
                productRepository.Update(product);
                productRepository.Save();
                OrderItem newItem = new OrderItem()
                {
                    ProductId = item.ProductID,
                    OrderId =(int)TempData["Order"],
                    Quantity = item.Quantity,
                    Price = cartRepository.GetTotalPriceOfOneItem(item)
                };
                orderItemsRepository.Insert(newItem);
                orderItemsRepository.Save();
            }
            return RedirectToAction("DeleteAll","Cart");
        }
    }
}
