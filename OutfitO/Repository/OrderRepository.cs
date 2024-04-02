using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public class OrderRepository:Repository<Order>, IOrderRepository
    {
        OutfitoContext _context;
        public OrderRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
		public List<Order> GetSomeOrders(int skip, int content)
		{
			return _context.Order.Include(o => o.User).Skip(skip).Take(content).ToList();
		}
		public List<Order> GetOrderForUSer(string userid)
        {
            return _context.Order.Where(o=>o.UserId == userid).ToList();    
        }
        public List<Order> GetSomeOrdersForUser(string userid,int skip, int content)
        {
            return _context.Order.Include(o => o.payment).Where(o => o.UserId == userid).Skip(skip).Take(content).ToList();
        }
        public List<OrderItem> GetOrderItem(int Orderid)
        {
            return _context.OrderItem
                           .Include(o => o.Product).Include(o => o.Order)
                           .Where(o=>o.OrderId==Orderid).OrderBy(o=>o.Order.Date)
                           .ToList();
        }
        public Payment GetPaymentForOrder(int Orderid)
        {
            var id= _context.Order.Where(p=>p.Id== Orderid).Select(p=>p.PaymentId).FirstOrDefault();
            return _context.Payment.Where(p=>p.Id==id).FirstOrDefault();
        }
        //
        public User GetUserInformation(int Orderid)
        {
            string userId = _context.Order.Where(p => p.Id == Orderid).Select(p => p.UserId).FirstOrDefault();
            return _context.User.Where(u => u.Id == userId).FirstOrDefault();
        }

        //Added 25-3
        public int CountOrdersForUser(string userid)
        {
            return _context.Order.Where(o => o.UserId == userid).Count();
        }

        public int GetOrderItemCount(int Orderid)
        {
            return _context.OrderItem.Where(o => o.OrderId == Orderid).Count();
        }
    }
}
