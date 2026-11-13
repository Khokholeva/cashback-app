using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public interface IDiscountCardEntity : INotificationEntity<int?>
    {
        public BarcodeResult Barcode { get; init; }
        public IShopEntity Shop { get; init; }

    }
}
