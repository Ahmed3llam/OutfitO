using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        public List<CartItem> GetCart(string id);
        public User GetUser(string id);
        public User CheckLogIn(string Email);
        public List<Product> GetProducts(string id);
        public List<Payment> GetPayment(string id);
        public List<Order> GetHistoryOfOrders(string id);
        public void DeleteUser(string id);

    }
}