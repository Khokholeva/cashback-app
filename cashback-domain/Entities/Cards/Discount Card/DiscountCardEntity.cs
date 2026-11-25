using cashback_domain.Entities.Cards.Discount_Card;
using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public class DiscountCardEntity : IDiscountCardEntity
    {
        public DiscountCardEntity(IShopEntity shop)
        {
            Shop = shop;
        }

        public int? Id { get; init; }
        public BarcodeResult? Barcode { get; set; }
        public IShopEntity Shop { get; init; }
        public DateTime? NextNotification { get; set; }
    }
}

