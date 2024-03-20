using OutfitO.Models;

namespace OutfitO.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        OutfitoContext _context;
        public PaymentRepository(OutfitoContext context):base(context)
        {
            _context = context;
        }
        public User GetUserInformation(int id)
        {
            string userId = _context.Payment.Where(p=>p.Id==id).Select(p => p.UserId).FirstOrDefault();
            return _context.User.Where(u => u.Id == userId).FirstOrDefault();
        }
        public List<Payment> GetPaymentForUSer(string id)
        {
            return _context.Payment.Where(p=>p.UserId==id).ToList();
        }
    }
}
