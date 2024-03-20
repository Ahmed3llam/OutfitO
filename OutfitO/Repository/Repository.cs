using OutfitO.Models;

namespace OutfitO.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        OutfitoContext _context;
        public Repository(OutfitoContext context)
        {
            _context = context;
        }
        public int Count()
        {
            return _context.Set<T>().Count();
        }
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public List<T> GetSome(int skip, int content)
        {
            return _context.Set<T>().Skip(skip).Take(content).ToList();
        }
        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Insert(T model)
        {
            _context.Add(model);
        }
        public void Update(T model)
        {
            _context.Update(model);
        }
        public void Delete(int id)
        {
            T model = Get(id);
            _context.Remove(model);

        }
        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
