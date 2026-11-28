using cashback_domain.Entities.Cards.Discount_Card;
using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public interface IDiscountCardEntity : INotificationEntity<int?>
    {
        public IBarcode? Barcode { get; }
        public IShopEntity Shop { get; }
        public string BackGroundColor { get; }
        public string Name { get; }

    }
}
