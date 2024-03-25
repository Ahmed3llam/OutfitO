using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface ICartRepository : IRepository<CartItem>
    {
        public List<CartItem> GetForUser(string UserId);
        public CartItem GetById(int productId, string UserId);
        public void Update(int productId, string UserId, CartItem item);
        public void Delete(int productId, string UserId);
        public void DeleteAll(string UserId);
        public decimal GetTotalPrice(string customerId);
        public decimal GetTotalPriceOfOneItem(CartItem item);
    }
}