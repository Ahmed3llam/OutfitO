using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IOrderItemsRepository:IRepository<OrderItem>
    {
        public Product GetProduct(int id);
        public Order GetOrder(int id);
    }
}