using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IOrderRepository:IRepository<Order>
    {
        public Payment GetPaymentForOrder(int orderid);
        public User GetUserInformation(int orderid);
        public List<OrderItem> GetOrderItem(int orderid);
        public List<Order> GetOrderForUSer(string userid);

    }
}