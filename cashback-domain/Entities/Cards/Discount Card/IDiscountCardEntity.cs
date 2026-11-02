using cashback_domain.Entities.Shop;
using IronBarCode;

namespace cashback_domain.Entities.Discount_Card
{
    internal interface IDiscountCardEntity : IEntity<int>
    {
        public BarcodeResult Barcode { get; init; }
        public IShopEntity Shop { get; init; }

    }
}
