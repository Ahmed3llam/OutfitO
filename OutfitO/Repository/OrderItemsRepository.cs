using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public class OrderItemsRepository: Repository<OrderItem>, IOrderItemsRepository
    {
        OutfitoContext _context;
        public OrderItemsRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            int Productid=_context.OrderItem.Where(o=>o.Id==id).Select(o=>o.ProductId).FirstOrDefault();
            return _context.Product.Where(p => p.Id == Productid).FirstOrDefault();
        }
        public Order GetOrder(int id)
        {
            int Orderid = _context.OrderItem.Where(o => o.Id == id).Select(o=>o.OrderId).FirstOrDefault();
            return _context.Order.Where(o=>o.Id==Orderid).FirstOrDefault();
        }

        public bool productExists(int productID,string userID) {
            return _context.OrderItem.Include(oi => oi.Order).Any(oi => oi.ProductId == productID && oi.Order.UserId == userID);
        }
    }
}
