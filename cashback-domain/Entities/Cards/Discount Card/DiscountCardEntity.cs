using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public class DiscountCardEntity : IDiscountCardEntity
    {
        public int? Id { get; set; }
        public BarcodeResult Barcode { get; init; }
        public IShopEntity Shop { get; init; }
        public DateTime NextNotification { get; set; }
    }
}

