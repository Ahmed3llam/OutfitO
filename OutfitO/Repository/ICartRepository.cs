using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface ICartRepository : IRepository<CartItem>
    {
        public List<CartItem> GetForUser(string UserId);
    }
}