using OutfitO.Models;

namespace OutfitO.Repository
{
    public class CartRepository:Repository<CartItem>, ICartRepository
    {
        OutfitoContext _context;
        public CartRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
        public List<CartItem> GetForUser(string Userid)
        {
            return _context.Cart.Where(p => p.UserID == Userid).ToList();
        }
    }
}
