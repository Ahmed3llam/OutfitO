using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IRepository<T>
    {
        public int Count();
        public List<T> GetAll();
        public List<T> GetSome(int skip, int content);
        public T Get(int id);
        public void Insert(T param);
        public void Update(T param);
        public void Delete(int id);
        public int Save();
    }
}
