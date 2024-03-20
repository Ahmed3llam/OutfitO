using OutfitO.Models;

namespace OutfitO.Repository
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        OutfitoContext _context;
        public CategoryRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
    }
}
