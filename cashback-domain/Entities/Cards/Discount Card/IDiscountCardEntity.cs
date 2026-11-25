using cashback_domain.Entities.Cards.Discount_Card;
using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public interface IDiscountCardEntity : INotificationEntity<int?>
    {
        public BarcodeResult? Barcode { get; set; }
        public IShopEntity Shop { get; init; }

    }
}
