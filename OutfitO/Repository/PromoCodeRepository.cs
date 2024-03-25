using OutfitO.Models;

namespace OutfitO.Repository
{
    public class PromoCodeRepository :Repository<PromoCode>, IPromoCodeRepository
    {
        OutfitoContext _context;
        public PromoCodeRepository(OutfitoContext context) : base(context)
        {
            _context = context;
        }
        public PromoCode GetPromoCode(string code)
        {
            return _context.Promo.Where(x => x.Code == code).FirstOrDefault();
        }
    }
}
