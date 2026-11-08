using cashback_domain.Entities.Cards.Bank_Card;
using cashback_domain.Entities.DB;
using cashback_domain.Entities.MCC;
using cashback_domain.Entities.Shop;
using System.Security.Cryptography;
using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public class DiscountCardEntity : IDiscountCardEntity
    {
        public BarcodeResult Barcode { get; init; }
        public IShopEntity Shop { get; init; }

        public void SaveToDB(DB.IDBRepository<DiscountCardEntity, int> dBRepository)
        {
            throw new NotImplementedException();
        }
    }
}
//
