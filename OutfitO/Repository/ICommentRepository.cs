using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface ICommentRepository:IRepository<Comment>
    {
        public List<Comment> GetForProduct(int ProductId);
        public List<Comment> GetForUser(string Userid);
    }
}