using Microsoft.EntityFrameworkCore;
using OutfitO.Models;
using OutfitO.ViewModels;

namespace OutfitO.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        OutfitoContext _context;
        public ProductRepository(OutfitoContext context) : base(context)
        {
            _context = context;
        }
        public List<Product> GetForCategory(int Categoryid)
        {
            return _context.Product.Where(p => p.CategoryId == Categoryid).ToList();
        }
        public List<Product> GetForCategory(string Category)
        {
            return _context.Product.Where(p => p.Title == Category).ToList();
        }
        public List<Product> GetForUser(int skip, int content, string Userid)
        {
            return _context.Product.Include(p=>p.Category).Include(p=>p.User).Where(p => p.UserID == Userid).Skip(skip).Take(content).ToList();
        }
        public Product GetProduct(int id)
        {
            return _context.Product.Include(c => c.Category).Include(c=>c.Comments).Where(p =>p.Id==id).FirstOrDefault();
        }
        public List<Product> GetSpeceficProduct(int skip, int content, List<Category> ParamCategory)
        {
            var allProducts = _context.Product.Include(p => p.Category).Include(p=>p.User).ToList();
            var filteredProducts = allProducts.Where(p => ParamCategory.Any(c => p.Category.Title.Contains(c.Title))).Skip(skip).Take(content).ToList();
            return filteredProducts;
        }
        public List<Product> GetSpeceficProduct(int skip, int content, List<string> ParamCategory)
        {
            var allProducts = _context.Product.Include(p => p.Category).Include(p => p.User).ToList();
            var filteredProducts = allProducts.Where(p => ParamCategory.Any(c => p.Category.Title.Contains(c))).Skip(skip).Take(content).ToList();
            return filteredProducts;
        }
        public int GetProductcount(List<Category> ParamCategory)
        {
            var allProducts = _context.Product.Include(p => p.Category).ToList();
            var filteredProducts = allProducts.Where(p => ParamCategory.Any(c => p.Category.Title.Contains(c.Title))).ToList();
            return filteredProducts.Count;
        }
        public int GetProductcount(List<string> ParamCategory)
        {
            var allProducts = _context.Product.Include(p => p.Category).ToList();
            var filteredProducts = allProducts.Where(p => ParamCategory.Any(c => p.Category.Title.Contains(c))).ToList();
            return filteredProducts.Count;
        }
        public List<Product> GetAll()
        {
            return _context.Product.ToList();
        }
        public Product GetById(int id)
        {
            return _context.Product.Include(i => i.Category).SingleOrDefault(p => p.Id == id);
        }
        public void insert(Product obj)
        {
            _context.Add(obj);
        }
        public void update(Product obj)
        {
            _context.Update(obj);
        }
        public void delete(int id)
        {
            Product product = GetById(id);
            _context.Remove(product);
        }
        public int save()
        {
            return _context.SaveChanges();
        }
        public void insert(ProductWithCategoryList product)
        {
            throw new NotImplementedException();
        }
    }
}
