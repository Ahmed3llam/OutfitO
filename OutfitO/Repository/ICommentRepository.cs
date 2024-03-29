using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public List<Comment> GetForProduct(int ProductId);
        public List<Comment> GetForUser(string Userid);

        public List<Comment> GetAll();

        public Comment GetById(int id);

        public void insert(Comment obj);
        public void update(Comment obj);

        public void delete(int id);


        public int save();
    }
}