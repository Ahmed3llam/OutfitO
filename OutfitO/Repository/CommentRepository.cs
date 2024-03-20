using OutfitO.Models;

namespace OutfitO.Repository
{
    public class CommentRepository:Repository<Comment>, ICommentRepository
    {
        OutfitoContext _context;
        public CommentRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
        public List<Comment> GetForProduct(int ProductId)
        {
            return _context.Comment.Where(c=>c.ProductID==ProductId).ToList();
        }
        public List<Comment> GetForUser(string Userid)
        {
            return _context.Comment.Where(c => c.UserId == Userid).ToList();
        }
    }
}
