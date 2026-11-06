using cashback_domain.Entities.Shop;
using ZXing.Net.Maui;

namespace cashback_domain.Entities.Discount_Card
{
    public interface IDiscountCardEntity : INotificationEntity<int>
    {
        public BarcodeResult Barcode { get; init; }
        public IShopEntity Shop { get; init; }

    }
}
