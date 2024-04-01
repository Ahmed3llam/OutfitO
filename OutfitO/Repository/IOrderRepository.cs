using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IOrderRepository:IRepository<Order>
    {
		public List<Order> GetSomeOrders(int skip, int content);
		public Payment GetPaymentForOrder(int orderid);
        public User GetUserInformation(int orderid);
        public List<OrderItem> GetOrderItem(int orderid);
        public List<Order> GetOrderForUSer(string userid);
        public int CountOrdersForUser(string userid);
        public int GetOrderItemCount(int Orderid);
        public List<Order> GetSomeOrdersForUser(string userid, int skip, int content);
    }
}