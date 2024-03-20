using OutfitO.Models;

namespace OutfitO.Repository
{
    public class ProductRepository:Repository<Product> , IProductRepository
    {
        OutfitoContext _context;
        public ProductRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
        public List<Product> GetForCategory(int Categoryid) {
            return _context.Product.Where(p=>p.CategoryId==Categoryid).ToList();
        }
        public List<Product> GetForCategory(string Category)
        {
            return _context.Product.Where(p => p.Title==Category).ToList();
        }
        public List<Product> GetForUser(string Userid)
        {
            return _context.Product.Where(p => p.UserID==Userid).ToList();
        }
    }
}
