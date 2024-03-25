using OutfitO.Models;

namespace OutfitO.Repository
{
    public interface IPromoCodeRepository:IRepository<PromoCode>
    {
        public PromoCode GetPromoCode(string code);
    }
}