using Microsoft.EntityFrameworkCore;
using OutfitO.Models;

namespace OutfitO.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        OutfitoContext _context;
        public PaymentRepository(OutfitoContext context) : base(context)
        {
            _context = context;
        }
        public Payment Get(string id)
        {
            return _context.Payment.Include(p=>p.User).Where(p => p.PaymentId == id).FirstOrDefault();
        }
        public User GetUserInformation(int id)
        {
            string userId = _context.Payment.Where(p => p.Id == id).Select(p => p.PaymentId).FirstOrDefault();
            return _context.User.Where(u => u.Id == userId).FirstOrDefault();
        }
        public List<Payment> GetPaymentForUSer(string id)
        {
            return _context.Payment.Where(p => p.PaymentId == id).ToList();
        }
    }
}
