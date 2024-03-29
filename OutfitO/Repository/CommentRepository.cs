using OutfitO.Models;

namespace OutfitO.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        OutfitoContext _context;
        public CommentRepository(OutfitoContext context) : base(context)
        {
            _context = context;
        }
        public List<Comment> GetForProduct(int ProductId)
        {
            return _context.Comment.Where(c => c.ProductID == ProductId).ToList();
        }
        public List<Comment> GetForUser(string Userid)
        {
            return _context.Comment.Where(c => c.UserId == Userid).ToList();
        }

        public List<Comment> GetAll()
        {
            return _context.Comment.ToList();
        }
        public Comment GetById(int id)
        {
            return _context.Comment.SingleOrDefault(c => c.Id == id);
        }

        public void insert(Comment obj)
        {
            _context.Add(obj);
        }
        public void update(Comment obj)
        {
            _context.Update(obj);
        }

        public void delete(int id)
        {
            Comment comment = _context.Comment.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
                _context.SaveChanges();
            }
        }


        public int save()
        {
            return _context.SaveChanges();
        }
    }
}
