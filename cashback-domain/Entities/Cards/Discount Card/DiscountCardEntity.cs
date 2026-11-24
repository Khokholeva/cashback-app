using ZXing.Net.Maui;

namespace cashback_domain.Entities
{
    public class DiscountCardEntity : IDiscountCardEntity
    {
        public DiscountCardEntity(BarcodeResult barcode, int shopID)
        {
            Barcode = barcode;
            ShopID = shopID;
        }

        public int? Id { get; init; }
        public BarcodeResult Barcode { get; init; }
        public int ShopID { get; init; }
        public DateTime? NextNotification { get; set; }
    }
}

