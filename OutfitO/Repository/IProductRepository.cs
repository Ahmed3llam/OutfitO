using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IProductRepository:IRepository<Product>
    {
        public List<Product> GetForCategory(int CategoryId);
        public List<Product> GetForUser(string Userid);
    }
}