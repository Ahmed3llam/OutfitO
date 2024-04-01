using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        public Payment Get(string id);
        public User GetUserInformation(int id);
        public List<Payment> GetPaymentForUSer(string id);
    }
}