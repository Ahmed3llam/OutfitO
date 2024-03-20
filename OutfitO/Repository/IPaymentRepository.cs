using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IPaymentRepository
    {
        public User GetUserInformation(int id);
        public List<Payment> GetPaymentForUSer(string id);
    }
}