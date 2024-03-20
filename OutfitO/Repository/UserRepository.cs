using OutfitO.Models;

namespace OutfitO.Repository
{
    public class UserRepository:Repository<User> ,IUserRepository
    {
        OutfitoContext _context;
        public UserRepository(OutfitoContext context) : base(context)
        {
            _context = context;
        }

        public User CheckLogIn(string Email)
        {
            return _context.User.Where(u => u.Email == Email).FirstOrDefault();
        }

        public List<CartItem> GetCart(string id)
        {
            return _context.Cart.Where(c => c.UserID == id).ToList();
        }

        public List<Order> GetHistoryOfOrders(string id)
        {
            //details for each order in order repo in GetOrderItem fn
            return _context.Order.Where(p => p.UserId == id).ToList();
        }

        public List<Payment> GetPayment(string id)
        {
            return _context.Payment.Where(p => p.UserId == id).ToList();
        }

        public List<Product> GetProducts(string id)
        {
            return _context.Product.Where(p=>p.UserID==id).ToList();
        }

        public User GetUser(string id)
        {
            return _context.User.Where(u=>u.Id == id).FirstOrDefault();
        }
        public void DeleteUser(string id)
        {
            User model = GetUser(id);
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == id);
            _context.UserRoles.RemoveRange(userRoles);
            _context.Remove(model);
        }
    }
}
