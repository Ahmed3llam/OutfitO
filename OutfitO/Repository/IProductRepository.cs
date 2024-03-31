using Microsoft.EntityFrameworkCore;
using OutfitO.Models;
using OutfitO.ViewModels;

namespace OutfitO.Repository
{
    public interface IProductRepository : IRepository<Product>
    {

        public List<Product> GetForCategory(int CategoryId);
        public List<Product> GetForUser(string Userid);

        public List<Product> GetSpeceficProduct(int skip, int content, List<Category> ParamCategory);

        public List<Product> GetSpeceficProduct(int skip, int content, List<string> ParamCategory);

        public int GetProductcount(List<Category> ParamCategory);

        public int GetProductcount(List<string> ParamCategory);

        public List<Product> GetAll();
        public Product GetById(int id);
        public void insert(Product obj);
        public void update(Product obj);
        public Product GetProduct(int id);
        public void delete(int id);

        public int save();
        void insert(ProductWithCategoryList product);
    }
}